
all:
	docker-compose run -w /app/Disunity.Store dotnet $(args)

build:
	docker-compose build

up: build
	docker-compose up db cache frontend web

ef:
	docker-compose run -w /app/Disunity.Store dotnet ef $(args)

dropdb:
	docker-compose run -w /app/Disunity.Store dotnet ef database drop

initdb:
	docker-compose run -w /app/Disunity.Store dotnet ef migrations add Initial -o Shared/Migrations

test:
	docker-compose run -w /app/Disunity.Store.Tests --entrypoint /app/Disunity.Store.Tests/start.sh dotnet

watcher:
	docker-volume-watcher -v --debounce 0.1 disunitystore_* ${CURDIR}/*
