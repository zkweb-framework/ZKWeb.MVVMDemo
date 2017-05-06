#!/usr/bin/env bash

old_project_name="ZKWeb.MVVMDemo"
read -p "please enter a new project name: " new_project_name

if [[ "x${new_project_name}" == "x" ]]; then
	echo "new project name can't be empty"
	exit
fi

find . -depth -type d -iname "*${old_project_name}*" -exec bash -c \
	"mv -v \"\$1\" \"\${1//${old_project_name}/${new_project_name}}\"" -- {} \;
find . -depth -type f -iname "*${old_project_name}*" -exec bash -c \
	"mv -v \"\$1\" \"\${1//${old_project_name}/${new_project_name}}\"" -- {} \;
find . -depth -type f -exec bash -c \
	"echo replace \"\$1\" && sed -i \"s/${old_project_name}/${new_project_name}/g\" \"\$1\"" -- {} \;
