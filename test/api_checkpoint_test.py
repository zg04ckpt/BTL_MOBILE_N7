import json
import os
import traceback
from dataclasses import dataclass
from datetime import datetime
from pathlib import Path
from typing import Any, Dict, List, Optional, Tuple
from urllib import error, parse, request


# -----------------------------
# Runtime configuration
# -----------------------------
BASE_URL = os.getenv("BASE_URL", "https://quizbattle.hoangcn.com").rstrip("/")
ACCESS_TOKEN = os.getenv("ACCESS_TOKEN", "")
TEST_EMAIL = os.getenv("TEST_EMAIL", "")
TEST_PASSWORD = os.getenv("TEST_PASSWORD", "")
RUN_WRITE_TESTS = os.getenv("RUN_WRITE_TESTS", "false").lower() == "true"
REQUEST_TIMEOUT = int(os.getenv("REQUEST_TIMEOUT", "20"))

USER_ID = os.getenv("USER_ID", "1")
ADMIN_ID = os.getenv("ADMIN_ID", "1")
TOPIC_ID = os.getenv("TOPIC_ID", "1")
QUESTION_ID = os.getenv("QUESTION_ID", "1")
EVENT_ID = os.getenv("EVENT_ID", "1")
MATCH_ID = os.getenv("MATCH_ID", "1")
SYSTEM_KEY = os.getenv("SYSTEM_KEY", "MaintenanceMode")

ROOT_DIR = Path(__file__).resolve().parent
LOG_DIR = ROOT_DIR / "log"
LOG_DIR.mkdir(parents=True, exist_ok=True)

RUN_LOG_PATH = LOG_DIR / "api_test_run.log"
SUMMARY_TXT_PATH = LOG_DIR / "api_test_summary.txt"
JSON_RESULT_PATH = LOG_DIR / "api_test_result.json"


@dataclass
class Checkpoint:
    cid: str
    method: str
    path: str
    group: str
    auth: str  # public | user | admin
    write: bool = False
    query: Optional[Dict[str, str]] = None
    json_body: Optional[Dict[str, Any]] = None


def build_url(path: str, query: Optional[Dict[str, str]] = None) -> str:
    url = f"{BASE_URL}{path}"
    if query:
        url = f"{url}?{parse.urlencode(query)}"
    return url


def safe_json_load(raw: str) -> Optional[Dict[str, Any]]:
    try:
        data = json.loads(raw)
        return data if isinstance(data, dict) else None
    except Exception:
        return None


def short_body(raw: str, max_len: int = 400) -> str:
    clean = raw.replace("\n", " ").replace("\r", " ")
    return clean[:max_len] + ("..." if len(clean) > max_len else "")


def http_call(
    method: str,
    path: str,
    query: Optional[Dict[str, str]] = None,
    headers: Optional[Dict[str, str]] = None,
    json_body: Optional[Dict[str, Any]] = None,
) -> Tuple[int, str]:
    url = build_url(path, query)
    req_headers = headers.copy() if headers else {}
    body_bytes = None

    if json_body is not None:
        body_bytes = json.dumps(json_body).encode("utf-8")
        req_headers["Content-Type"] = "application/json"

    req = request.Request(url=url, data=body_bytes, headers=req_headers, method=method)

    try:
        with request.urlopen(req, timeout=REQUEST_TIMEOUT) as resp:
            payload = resp.read().decode("utf-8", errors="replace")
            return resp.getcode(), payload
    except error.HTTPError as e:
        payload = e.read().decode("utf-8", errors="replace")
        return e.code, payload
    except Exception as e:
        return 0, f"EXCEPTION: {type(e).__name__}: {e}"


def eval_result(cp: Checkpoint, status: int, has_token: bool) -> Tuple[str, str]:
    if status == 0:
        return "FAIL", "Request exception/timeout"

    if cp.write and not RUN_WRITE_TESTS:
        return "SKIP", "Write test skipped (set RUN_WRITE_TESTS=true to execute)"

    if cp.auth == "public":
        if 200 <= status < 300:
            return "PASS", "Public endpoint returns success"
        return "FAIL", f"Expected 2xx for public endpoint, got {status}"

    if cp.auth in {"user", "admin"}:
        # If token exists, expect normal success (2xx).
        # If token missing, unauthorized response (401/403) is acceptable for checkpoint.
        if has_token:
            if 200 <= status < 300:
                return "PASS", "Authorized endpoint success with token"
            return "FAIL", f"Token provided but got {status}"
        if status in (401, 403):
            return "PASS", "Auth guard works (no token -> unauthorized)"
        if 200 <= status < 300:
            return "PASS", "Endpoint allowed without token in current deployment"
        return "FAIL", f"Expected 401/403 or 2xx, got {status}"

    return "FAIL", "Unknown auth mode"


def make_checkpoints() -> List[Checkpoint]:
    return [
        # Auth
        Checkpoint("AUTH_LOGIN", "POST", "/api/auth/login", "Auth", "public", json_body={
            "email": TEST_EMAIL or "",
            "password": TEST_PASSWORD or "",
        }),
        Checkpoint("AUTH_REGISTER", "POST", "/api/auth/register", "Auth", "public", write=True, json_body={
            "displayName": "api_test_user",
            "email": f"api_test_{int(datetime.utcnow().timestamp())}@example.com",
            "phoneNumber": "0900000000",
            "password": "Test@12345",
        }),

        # Users
        Checkpoint("USERS_GET_ALL", "GET", "/api/users", "Users", "public"),
        Checkpoint("USERS_PAGING", "GET", "/api/users/paging", "Users", "public", query={"pageIndex": "1", "pageSize": "10"}),
        Checkpoint("USERS_GET_BY_ID", "GET", f"/api/users/{USER_ID}", "Users", "public"),
        Checkpoint("USERS_PROFILE_GET", "GET", "/api/users/profile", "Users", "user"),
        Checkpoint("USERS_CREATE", "POST", "/api/users", "Users", "public", write=True, json_body={
            "name": "api_created_user",
            "email": f"api_created_{int(datetime.utcnow().timestamp())}@example.com",
            "phoneNumber": "0911111111",
            "password": "Test@12345",
            "roleId": 2,
        }),
        Checkpoint("USERS_UPDATE", "PUT", f"/api/users/{USER_ID}", "Users", "public", write=True, json_body={
            "name": "api_updated_user",
        }),
        Checkpoint("USERS_DELETE", "DELETE", f"/api/users/{USER_ID}", "Users", "public", write=True),

        # Topics
        Checkpoint("TOPICS_GET_ALL", "GET", "/api/topics", "Topics", "public"),
        Checkpoint("TOPICS_PAGING", "GET", "/api/topics/paging", "Topics", "public", query={"pageIndex": "1", "pageSize": "10"}),
        Checkpoint("TOPICS_GET_BY_ID", "GET", f"/api/topics/{TOPIC_ID}", "Topics", "public"),
        Checkpoint("TOPICS_CREATE", "POST", "/api/topics", "Topics", "admin", write=True, json_body={
            "name": "API Test Topic",
            "description": "Created by automated checkpoint test",
        }),
        Checkpoint("TOPICS_UPDATE", "PUT", f"/api/topics/{TOPIC_ID}", "Topics", "admin", write=True, json_body={
            "name": "API Test Topic Updated",
            "description": "Updated by automated checkpoint test",
        }),
        Checkpoint("TOPICS_DELETE", "DELETE", f"/api/topics/{TOPIC_ID}", "Topics", "admin", write=True),

        # Questions
        Checkpoint("QUESTIONS_PAGING", "GET", "/api/questions/paging", "Questions", "public", query={"pageIndex": "1", "pageSize": "10"}),
        Checkpoint("QUESTIONS_GET_BY_ID", "GET", f"/api/questions/{QUESTION_ID}", "Questions", "public"),
        Checkpoint("QUESTIONS_CREATE", "POST", "/api/questions", "Questions", "admin", write=True, json_body={
            "content": "API test question",
            "topicId": int(TOPIC_ID) if TOPIC_ID.isdigit() else 1,
            "level": 1,
            "type": 1,
            "answers": "[]",
        }),
        Checkpoint("QUESTIONS_UPDATE", "PUT", f"/api/questions/{QUESTION_ID}", "Questions", "admin", write=True, json_body={
            "content": "API test question updated",
        }),
        Checkpoint("QUESTIONS_DELETE", "DELETE", f"/api/questions/{QUESTION_ID}", "Questions", "admin", write=True),

        # Battles
        Checkpoint("BATTLES_START_OPTIONS", "GET", "/api/battles/start-options", "Battles", "public"),
        Checkpoint("BATTLES_PAGING", "GET", "/api/battles/paging", "Battles", "public", query={"pageIndex": "1", "pageSize": "10"}),
        Checkpoint("BATTLES_GET_BY_ID", "GET", f"/api/battles/{MATCH_ID}", "Battles", "public"),
        Checkpoint("BATTLES_DELETE", "DELETE", f"/api/battles/{MATCH_ID}", "Battles", "public", write=True),
        Checkpoint("BATTLES_MATCH_INFO", "GET", "/api/battles/match-info", "Battles", "user"),
        Checkpoint("BATTLES_MATCH_RESULT", "GET", "/api/battles/match-result", "Battles", "user"),
        Checkpoint("BATTLES_LOBBY_JOIN", "POST", "/api/battles/lobbies/join", "Battles", "user", write=True, json_body={
            "topicId": int(TOPIC_ID) if TOPIC_ID.isdigit() else 1,
            "type": 1,
        }),
        Checkpoint("BATTLES_LOBBY_OUT", "POST", "/api/battles/lobbies/out", "Battles", "user", write=True, json_body={
            "lobbyRoomId": "test",
        }),

        # Events
        Checkpoint("EVENTS_GET_ALL", "GET", "/api/events", "Events", "public"),
        Checkpoint("EVENTS_REWARD_MAPPINGS", "GET", "/api/events/rewards/mappings", "Events", "public"),
        Checkpoint("EVENTS_USER_PROGRESSES", "GET", f"/api/events/users/{USER_ID}/progresses", "Events", "public"),
        Checkpoint("EVENTS_MY_PROGRESSES", "GET", "/api/events/users/my-progresses", "Events", "user"),
        Checkpoint("EVENTS_MY_PROGRESS_UPDATE", "PUT", "/api/events/my-progress", "Events", "user", write=True, json_body={
            "eventId": int(EVENT_ID) if EVENT_ID.isdigit() else 1,
            "progress": 1,
        }),
        Checkpoint("EVENTS_CREATE", "POST", "/api/events", "Events", "public", write=True, json_body={
            "name": "API Test Event",
            "description": "Created by automated checkpoint test",
        }),
        Checkpoint("EVENTS_UPDATE", "PUT", f"/api/events/{EVENT_ID}", "Events", "public", write=True, json_body={
            "name": "API Test Event Updated",
        }),
        Checkpoint("EVENTS_DELETE", "DELETE", f"/api/events/{EVENT_ID}", "Events", "public", write=True),

        # Rank
        Checkpoint("RANK_TYPES", "GET", "/api/rank/types", "Rank", "public"),
        Checkpoint("RANK_BOARD", "GET", "/api/rank/board", "Rank", "public", query={"type": "Monthly", "pageIndex": "1", "pageSize": "10"}),
        Checkpoint("RANK_BY_USER", "GET", f"/api/rank/{USER_ID}", "Rank", "public"),

        # Analytics
        Checkpoint("ANALYTICS_GET", "GET", "/api/analytics", "Analytics", "public"),
        Checkpoint("ANALYTICS_RECENT_USERS", "GET", "/api/analytics/recent-users", "Analytics", "public"),
        Checkpoint("ANALYTICS_EXPORT", "GET", "/api/analytics/export", "Analytics", "public"),

        # System Config
        Checkpoint("SYSCONFIG_GET_ALL", "GET", "/api/systemconfigurations", "SystemConfig", "public"),
        Checkpoint("SYSCONFIG_UPDATE", "PUT", "/api/systemconfigurations", "SystemConfig", "public", write=True, json_body={
            "maintenanceMode": False,
        }),
        Checkpoint("SYSCONFIG_GET_KEY", "GET", f"/api/systemconfigurations/{SYSTEM_KEY}", "SystemConfig", "public"),

        # Enums
        Checkpoint("ENUMS_GET", "GET", "/api/enums", "Enums", "public"),

        # Tests
        Checkpoint("TESTS_HEALTH", "GET", "/api/tests", "Tests", "public"),
        Checkpoint("TESTS_REALTIME_CREATE", "GET", "/api/tests/realtime/create-test-lobby-room", "Tests", "public", write=True),
        Checkpoint("TESTS_REALTIME_ADD_PLAYER", "GET", "/api/tests/realtime/add-test-player-to-test-lobby-room", "Tests", "public", write=True),
    ]


def try_login_for_token() -> Tuple[str, Optional[str], int, str]:
    if ACCESS_TOKEN:
        return "PASS", ACCESS_TOKEN, 0, "Using ACCESS_TOKEN from environment"

    if not TEST_EMAIL or not TEST_PASSWORD:
        return "SKIP", None, 0, "No credentials provided (set TEST_EMAIL and TEST_PASSWORD)"

    status, payload = http_call(
        method="POST",
        path="/api/auth/login",
        json_body={"email": TEST_EMAIL, "password": TEST_PASSWORD},
    )

    if not (200 <= status < 300):
        return "FAIL", None, status, f"Login failed: {short_body(payload)}"

    parsed = safe_json_load(payload)
    token = None
    if parsed:
        data = parsed.get("data")
        if isinstance(data, dict):
            token = data.get("accessToken")

    if token:
        return "PASS", token, status, "Token acquired from /api/auth/login"

    return "FAIL", None, status, "Login success but accessToken not found in response.data"


def run() -> int:
    started = datetime.utcnow()
    checkpoints = make_checkpoints()

    lines: List[str] = []
    result_rows: List[Dict[str, Any]] = []

    lines.append(f"Run time (UTC): {started.isoformat()}Z")
    lines.append(f"Base URL: {BASE_URL}")
    lines.append(f"Run write tests: {RUN_WRITE_TESTS}")
    lines.append(f"Total checkpoints configured: {len(checkpoints)}")
    lines.append("-")

    login_state, token, login_status, login_note = try_login_for_token()
    lines.append(f"[LOGIN_SETUP] status={login_state} http={login_status} note={login_note}")

    pass_count = 0
    fail_count = 0
    skip_count = 0

    for cp in checkpoints:
        headers = {}
        if cp.auth in {"user", "admin"} and token:
            headers["Authorization"] = f"Bearer {token}"

        if cp.write and not RUN_WRITE_TESTS:
            outcome = "SKIP"
            note = "Write checkpoint skipped by policy"
            status = -1
            payload = ""
        else:
            status, payload = http_call(
                method=cp.method,
                path=cp.path,
                query=cp.query,
                headers=headers,
                json_body=cp.json_body,
            )
            outcome, note = eval_result(cp, status, bool(token))

        if outcome == "PASS":
            pass_count += 1
        elif outcome == "FAIL":
            fail_count += 1
        else:
            skip_count += 1

        row = {
            "id": cp.cid,
            "group": cp.group,
            "method": cp.method,
            "path": cp.path,
            "auth": cp.auth,
            "write": cp.write,
            "status": status,
            "result": outcome,
            "note": note,
            "response_sample": short_body(payload),
        }
        result_rows.append(row)

        lines.append(
            f"[{outcome}] {cp.cid} {cp.method} {cp.path} "
            f"(auth={cp.auth}, write={cp.write}) -> http={status}; {note}"
        )

    ended = datetime.utcnow()
    duration_sec = (ended - started).total_seconds()

    summary_lines = [
        "QUIZBATTLE API TEST SUMMARY",
        f"Started (UTC): {started.isoformat()}Z",
        f"Ended   (UTC): {ended.isoformat()}Z",
        f"Duration (s): {duration_sec:.2f}",
        f"Base URL: {BASE_URL}",
        f"Run write tests: {RUN_WRITE_TESTS}",
        f"Total: {len(checkpoints)}",
        f"PASS : {pass_count}",
        f"FAIL : {fail_count}",
        f"SKIP : {skip_count}",
        "",
        "Failed checkpoints:",
    ]

    for r in result_rows:
        if r["result"] == "FAIL":
            summary_lines.append(
                f"- {r['id']} [{r['method']} {r['path']}] http={r['status']} note={r['note']}"
            )

    if fail_count == 0:
        summary_lines.append("- None")

    summary_lines.append("")
    summary_lines.append(f"Detailed run log: {RUN_LOG_PATH}")
    summary_lines.append(f"JSON results    : {JSON_RESULT_PATH}")

    try:
        RUN_LOG_PATH.write_text("\n".join(lines) + "\n", encoding="utf-8")
        SUMMARY_TXT_PATH.write_text("\n".join(summary_lines) + "\n", encoding="utf-8")
        JSON_RESULT_PATH.write_text(json.dumps(result_rows, indent=2, ensure_ascii=False), encoding="utf-8")
    except Exception:
        print("Failed writing log files")
        traceback.print_exc()
        return 2

    print("\n".join(summary_lines))
    # Non-zero when any failure for CI-like behavior.
    return 1 if fail_count > 0 else 0


if __name__ == "__main__":
    raise SystemExit(run())
