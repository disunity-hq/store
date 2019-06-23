
all:
	docker-compose run dotnet $(args)

build:
	docker-compose build

up: build
	docker-compose up db cache web

ef:
	docker-compose run dotnet ef $(args)
