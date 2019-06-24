#!/usr/bin/env bash

mkdir /stage

cp -r /app/* /stage

cd /stage

dotnet test
