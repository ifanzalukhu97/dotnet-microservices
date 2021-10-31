## Tentang
Kode di repository ini adalah hasil latihan dari [course Youtube Les Jackson](https://www.youtube.com/watch?v=DgVjEo3OGBI) yang membahas tentang implementasi microservices di .NET Core.

## Beberapa Teknologi Yang Digunakan
- .NET 5
- Docker & Kubernetes
- RabbitMQ
- gRPC
- Microsoft SQL Server

## Cara Memulai Service di Local
Pastikan Docker dan Kubernetes sudah terinstall di local machine / laptop Anda. Untuk pengguna Windows bisa menggunakan **Docker Desktop** untuk install Docker dan Kubernetes.
Sedangkan untuk pengguna Linux bisa menggunakan **Minikube** untuk kubernetesnya.

1. Setelah clone project di atas, buka terminal dan change directory ke **K8S/**
2. Pertama apply RabbitMQ dengan perintah 
   
    `kubectl apply -f rabbitmq-depl.yaml`

3. Selanjutnya apply SQL Server dengan perintah

    `kubectl apply -f mssql-platform-depl.yaml && kubectl apply -f local-pvc.yaml`

4. Selanjutnya apply Platform Service dengan perintah

    `kubectl apply -f platforms-depl.yaml && kubectl apply -f platforms-np-srv.yaml`

5. Selanjutnya apply Command Service dengan perintah

    `kubectl apply -f commands-depl.yaml`

6. Selanjutnya apply Nginx sebagai gateway dengan perintah 

    `kubectl apply -f ingress-srv.yaml`

## Url & Port
* Karena menggunakan Nginx sebagai gateway, disini saya menggunakan base urlnya dgn 
  * ifanzalukhu97.me
* Jadi disini saya melakukan host mapping dari url Nginx ke ifanzalukhu97.me. Cara mappingnya sendiri, bisa dilihat caranya di bawah
  * [Pengguna Windows](https://docs.rackspace.com/support/how-to/modify-your-hosts-file/)
  * [Pengguna Linux](https://unix.stackexchange.com/questions/109245/hostname-and-ip-address-mapping-in-etc-hosts)
