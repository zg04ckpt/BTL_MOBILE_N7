docker compose down
docker rmi quizbattle_web
docker load -i new.tar 
docker compose up -d