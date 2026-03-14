using Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models;
using System.ComponentModel;
using System.Reflection;
using System.Text.RegularExpressions;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class EnumsController : ControllerBase
    {
        private static readonly IReadOnlyDictionary<string, string> EnumDescriptions =
            new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                ["RoleName"] = "Các cấp vai trò người dùng dùng cho phân quyền.",
                ["AccountStatus"] = "Trạng thái vòng đời hiện tại của tài khoản.",
                ["ConfigurationKey"] = "Các khóa cấu hình hệ thống do quản trị viên quản lý.",
                ["QuestionType"] = "Định dạng trả lời của câu hỏi.",
                ["QuestionStatus"] = "Trạng thái duyệt và xuất bản câu hỏi.",
                ["QuestionLevel"] = "Mức độ khó của câu hỏi.",
                ["RankingType"] = "Loại chu kỳ của bảng xếp hạng.",
                ["BattleType"] = "Chế độ thi đấu của trận.",
                ["LobbyRoomStatus"] = "Trạng thái hiện tại của phòng chờ.",
                ["MatchContentType"] = "Cách chọn nội dung câu hỏi cho trận đấu.",
                ["MatchStatus"] = "Trạng thái hiện tại của trận đấu.",
                ["UserInMatchStatus"] = "Trạng thái hiện tại của người chơi trong trận.",
                ["EventType"] = "Các loại mẫu sự kiện được hỗ trợ.",
                ["EventTimeType"] = "Cách cấu hình thời lượng và chu kỳ lặp của sự kiện.",
                ["EventStatus"] = "Trạng thái vòng đời hiện tại của sự kiện.",
                ["EventRewardType"] = "Các nhóm phần thưởng được trao từ sự kiện.",
                ["TournamentRewardsTaskType"] = "Các mục tiêu nhiệm vụ dùng trong sự kiện thưởng giải đấu."
            };

        private static readonly IReadOnlyDictionary<string, string> EnumValueDescriptions =
            new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                ["RoleName.SuperAdmin"] = "Vai trò có đặc quyền cao nhất với toàn quyền hệ thống.",
                ["RoleName.Admin"] = "Vai trò quản trị với quyền quản lý rộng.",
                ["RoleName.Moderator"] = "Vai trò kiểm duyệt hoạt động người dùng và nội dung.",
                ["RoleName.Editor"] = "Vai trò tạo mới hoặc cập nhật nội dung quản lý.",
                ["RoleName.User"] = "Vai trò mặc định của người dùng cuối.",

                ["AccountStatus.Active"] = "Tài khoản đang hoạt động và có thể dùng các tính năng được phép.",
                ["AccountStatus.Inactive"] = "Tài khoản tồn tại nhưng đang tạm ngưng hoạt động.",
                ["AccountStatus.Banned"] = "Tài khoản bị khóa do vi phạm chính sách.",
                ["AccountStatus.Deleted"] = "Tài khoản đã bị xóa mềm hoặc không còn khả dụng.",

                ["ConfigurationKey.MaintenanceMode"] = "Bật hoặc tắt chế độ bảo trì.",
                ["ConfigurationKey.WhitelistIPs"] = "Danh sách IP được phép truy cập khi bảo trì.",
                ["ConfigurationKey.RequiredAppVersion"] = "Phiên bản ứng dụng tối thiểu mà client phải dùng.",
                ["ConfigurationKey.LoginLiveTime"] = "Thời gian sống của phiên đăng nhập.",
                ["ConfigurationKey.QuestionTimeLimit"] = "Giới hạn thời gian trả lời mỗi câu hỏi.",
                ["ConfigurationKey.QuestionsPerMatch"] = "Số lượng câu hỏi trong một trận đấu.",
                ["ConfigurationKey.BaseWinPoints"] = "Điểm cơ bản được cộng khi thắng.",
                ["ConfigurationKey.BaseLosePoints"] = "Điểm cơ bản được cộng hoặc trừ khi thua.",
                ["ConfigurationKey.EloKFactor"] = "Hệ số K của ELO dùng để cập nhật điểm xếp hạng.",

                ["QuestionType.SingleChoice"] = "Một đáp án đúng trong nhiều lựa chọn.",
                ["QuestionType.MultipleChoice"] = "Có thể chọn nhiều đáp án đúng.",
                ["QuestionType.TrueFalse"] = "Dạng câu hỏi đúng hoặc sai.",

                ["QuestionStatus.Draft"] = "Câu hỏi đang được soạn và chưa gửi duyệt.",
                ["QuestionStatus.Pending"] = "Câu hỏi đang chờ kiểm duyệt.",
                ["QuestionStatus.Approved"] = "Câu hỏi đã được duyệt và có thể sử dụng.",
                ["QuestionStatus.Rejected"] = "Câu hỏi đã bị từ chối sau khi duyệt.",

                ["QuestionLevel.Normal"] = "Mức độ dễ hoặc tiêu chuẩn.",
                ["QuestionLevel.Medium"] = "Mức độ trung bình.",
                ["QuestionLevel.Hard"] = "Mức độ khó cao.",

                ["RankingType.Monthly"] = "Bảng xếp hạng tổng hợp theo tháng.",
                ["RankingType.Yearly"] = "Bảng xếp hạng tổng hợp theo năm.",
                ["RankingType.Total"] = "Bảng xếp hạng tổng hợp toàn thời gian.",

                ["BattleType.Single"] = "Chế độ đấu đơn, mỗi bên một người.",
                ["BattleType.Team"] = "Chế độ thi đấu theo đội.",

                ["LobbyRoomStatus.InQueue"] = "Phòng chờ đang đợi đủ người chơi.",
                ["LobbyRoomStatus.Completed"] = "Phòng chờ đã hoàn tất và trận đã bắt đầu hoặc kết thúc.",
                ["LobbyRoomStatus.Timeout"] = "Phòng chờ hết hạn do quá thời gian.",
                ["LobbyRoomStatus.Failed"] = "Phòng chờ không thể tạo trận hợp lệ.",

                ["MatchContentType.Random"] = "Câu hỏi được chọn ngẫu nhiên.",
                ["MatchContentType.Mix"] = "Câu hỏi được trộn từ nhiều nguồn hoặc chủ đề.",
                ["MatchContentType.OnlyOne"] = "Câu hỏi chỉ được chọn từ một nguồn hoặc một chủ đề.",

                ["MatchStatus.InProgress"] = "Trận đấu đang diễn ra.",
                ["MatchStatus.Ended"] = "Trận đấu đã kết thúc.",

                ["UserInMatchStatus.InMatch"] = "Người chơi đang ở trong một trận đang diễn ra.",
                ["UserInMatchStatus.LostConnect"] = "Người chơi bị mất kết nối trong trận.",
                ["UserInMatchStatus.Finished"] = "Người chơi đã hoàn thành trận đấu.",
                ["UserInMatchStatus.EarlyOut"] = "Người chơi rời trận trước khi hoàn tất.",

                ["EventType.QuizMilestoneChallenge"] = "Sự kiện dựa trên các mốc tiến độ làm quiz.",
                ["EventType.LuckySpin"] = "Sự kiện nhận thưởng thông qua quay may mắn.",
                ["EventType.TournamentRewards"] = "Sự kiện trao thưởng theo nhiệm vụ giải đấu.",

                ["EventTimeType.Limited"] = "Sự kiện diễn ra trong khoảng thời gian bắt đầu-kết thúc cố định.",
                ["EventTimeType.Daily"] = "Sự kiện đặt lại và lặp lại hằng ngày.",
                ["EventTimeType.Seasonal"] = "Sự kiện diễn ra theo mùa.",

                ["EventStatus.Pending"] = "Sự kiện đã được cấu hình nhưng chưa bắt đầu.",
                ["EventStatus.InProgress"] = "Sự kiện đang hoạt động.",
                ["EventStatus.Ended"] = "Sự kiện đã kết thúc.",

                ["EventRewardType.RankProtectionCard"] = "Vật phẩm bảo vệ điểm xếp hạng khỏi bị trừ.",
                ["EventRewardType.ExpScore"] = "Phần thưởng điểm kinh nghiệm.",
                ["EventRewardType.Gold"] = "Phần thưởng tiền vàng trong game.",
                ["EventRewardType.MatchLoudspeaker"] = "Vật phẩm loa phát thông báo trận đấu.",

                ["TournamentRewardsTaskType.PlayMatch"] = "Nhiệm vụ yêu cầu tham gia các trận đấu.",
                ["TournamentRewardsTaskType.WinMatch"] = "Nhiệm vụ yêu cầu giành chiến thắng các trận đấu.",
                ["TournamentRewardsTaskType.LoseMatch"] = "Nhiệm vụ tính các trận kết thúc với kết quả thua."
            };

        [HttpGet]
        public IActionResult GetAllEnums()
        {
            var enums = typeof(RefPoint).Assembly
                .GetTypes()
                .Where(type => type.IsEnum
                    && type.Namespace != null
                    && type.Namespace.StartsWith("Models.", StringComparison.Ordinal)
                    && type.Namespace.Contains(".Enums", StringComparison.Ordinal))
                .OrderBy(type => type.Namespace)
                .ThenBy(type => type.Name)
                .Select(BuildEnumDto)
                .ToArray();

            return Ok(ApiResponse.Success(enums));
        }

        private static EnumDto BuildEnumDto(Type enumType)
        {
            var values = Enum.GetValues(enumType)
                .Cast<object>()
                .Select(value =>
                {
                    var valueName = Enum.GetName(enumType, value) ?? value.ToString() ?? string.Empty;
                    return new EnumValueDto
                    {
                        Name = valueName,
                        Value = Convert.ToInt32(value),
                        Description = ResolveEnumValueDescription(enumType, valueName)
                    };
                })
                .ToArray();

            return new EnumDto
            {
                Name = enumType.Name,
                Namespace = enumType.Namespace ?? string.Empty,
                Description = ResolveEnumDescription(enumType),
                Values = values
            };
        }

        private static string ResolveEnumDescription(MemberInfo enumType)
        {
            var fromAttribute = enumType.GetCustomAttribute<DescriptionAttribute>()?.Description;
            if (!string.IsNullOrWhiteSpace(fromAttribute))
            {
                return fromAttribute;
            }

            if (EnumDescriptions.TryGetValue(enumType.Name, out var mappedDescription))
            {
                return mappedDescription;
            }

            return Humanize(enumType.Name);
        }

        private static string ResolveEnumValueDescription(Type enumType, string valueName)
        {
            var valueMember = enumType.GetMember(valueName).FirstOrDefault();
            var fromAttribute = valueMember?.GetCustomAttribute<DescriptionAttribute>()?.Description;
            if (!string.IsNullOrWhiteSpace(fromAttribute))
            {
                return fromAttribute;
            }

            var key = $"{enumType.Name}.{valueName}";
            if (EnumValueDescriptions.TryGetValue(key, out var mappedDescription))
            {
                return mappedDescription;
            }

            return Humanize(valueName);
        }

        private static string Humanize(string text)
        {
            return Regex.Replace(text, "(?<!^)([A-Z])", " $1").Trim();
        }

        private sealed class EnumDto
        {
            public string Name { get; init; } = string.Empty;
            public string Namespace { get; init; } = string.Empty;
            public string Description { get; init; } = string.Empty;
            public IReadOnlyCollection<EnumValueDto> Values { get; init; } = Array.Empty<EnumValueDto>();
        }

        private sealed class EnumValueDto
        {
            public string Name { get; init; } = string.Empty;
            public int Value { get; init; }
            public string Description { get; init; } = string.Empty;
        }
    }
}