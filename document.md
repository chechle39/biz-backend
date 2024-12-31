The single responsibility principle

gather together those things that change for the same reason, and separate those things that change for different reasons

======
dotnet new webapi -n PlatformService
=====
== build and push docke hub ==
docker build -t tuanlt6/platformservice .
docker run -p 8080:8080 -d tuanlt6/platformservice
docker ps => follow docker đang runing
docker stop 1e13aef68ff3 => turn off container is running
docker start 1e13aef68ff3 => start container again
docker push tuanlt6/platformservice => push image to docker hub


