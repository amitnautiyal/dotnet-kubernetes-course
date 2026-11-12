## Troubleshooting

### Docker Does Not Work

First make sure Rancher Desktop is running.

Then verify that the Moby/Docker runtime is selected.

Run:

```powershell
docker --version
```

If the Docker client works but the Docker server cannot be reached, Rancher Desktop may not be running or the container runtime may not be ready.

---

### Docker Engine Is Running but Commands Behave Unexpectedly

Check:

```powershell
docker info
```

Look at the Docker server information.

Also verify which containers and images are available:

```powershell
docker ps
```

```powershell
docker images
```

---

### Kubernetes Does Not Work

First make sure Kubernetes is enabled in Rancher Desktop.

Wait for Kubernetes to finish starting.

Then run:

```powershell
kubectl get nodes
```

If the cluster is still starting, wait a little longer and try again.

---

### kubectl Is Not Available

Run:

```powershell
kubectl version --client
```

If the command is not found, verify that kubectl is installed and available on your PATH.

Rancher Desktop documentation:

https://docs.rancherdesktop.io/

---

### Docker Images Are Missing

If you switch between container runtimes, image stores may be different.

For this course, stay with:

```text
Moby + Docker CLI
```

Avoid switching runtimes in the middle of the course unless you understand the implications.

---

## Verification Checklist

Before starting Day 1, verify the following.

### 1. WSL2

```powershell
wsl -l -v
```

Your Linux distribution should show:

```text
VERSION
2
```

### 2. Docker Client and Server

```powershell
docker version
```

Both Client and Server information should be available.

### 3. Docker Engine

```powershell
docker info
```

### 4. Docker Test Container

```powershell
docker run --rm hello-world
```

### 5. kubectl

```powershell
kubectl version --client
```

### 6. Kubernetes

```powershell
kubectl get nodes
```

At least one node should show:

```text
Ready
```

If all six checks work, your local development environment is ready for the course.
