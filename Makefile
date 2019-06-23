


build:
	docker-compose build

up: build
	docker-compose up db cache web

dotnet:
	docker-compose run dotnet
