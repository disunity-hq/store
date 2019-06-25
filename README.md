# Disunity Store
This repo contains the source for the disunity.io store.

## Setup guide for development
 * (Windows only see [below](#setup-for-windows-users)): Run `pip install docker-windows-volume-watcher`
 * Copy `.env.template` to `.env` and modify as necessary
 * Run `make up`
 * (Windows only) After all services have started, run `make watcher`

#### Setup for Windows Users
 Currently docker containers do not properly receive file change notifications from
 Windows, as such [docker-windows-volume-watcher](https://github.com/merofeev/docker-windows-volume-watcher) is necessary for file watching in development mode to work.


## Environment Variables
The Disunity Store is mainly configured through `appsettings.json`, however for
changes that need to be provided from the environment `appsettings.json` (namely secrets)
environment variables are available.

|           Variable | Description                                                  |
|:-------------------|--------------------------------------------------------------|
| SYNCFUSION_LICENSE | The license key for [Syncfusion](https://www.syncfusion.com) |
| ADMINUSER_EMAIL    | The email address to be used for the generated admin user    |
| ADMINUSER_PASSWORD | The password to be used for the generated admin user         |

If either `ADMINUSER_EMAIL` or `ADMINUSER_PASSWORD` aren't specified, an admin user won't be created
