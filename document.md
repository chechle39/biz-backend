The single responsibility principle

gather together those things that change for the same reason, and separate those things that change for different reasons

======
dotnet new webapi -n PlatformService
=====
== build and push docke hub ==
docker build -t tuanlt6/platformservice .
docker run -p 8080:8080 -d tuanlt6/platformservice
docker ps => follow docker is runing
docker stop 1e13aef68ff3 => turn off container is running
docker start 1e13aef68ff3 => start container again
docker push tuanlt6/platformservice => push image to docker hub


==K8s==
kubectl version
kubectl apply -f Platforms-depl.yaml => created k8s container
kubectl get deployment =>  check container running
kubectl get pods =>  check pods runing
each pod will has a container

kubectl delete deployment platforms-depl => delete container and pods
kubectl rollout restart deployment platforms-depl => restart and pull new package from docker hub

==NodePort==
kubectl apply -f platforms-np-srv.yaml => apply node || must turn on the Platforms-depl.yaml
kubectl get services =>check node available
kubectl delete services platformnpservice-srv => delete NodePort