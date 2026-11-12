# Rancher Desktop Setup

## What is Rancher Desktop?

Rancher Desktop provides a local development environment for working with containers and Kubernetes.

It can provide:

- Docker-compatible container tooling
- Moby / Docker
- containerd
- nerdctl
- kubectl
- Helm
- Local Kubernetes

For this course, we will use the **Docker CLI with the Moby container engine** for the Docker exercises.

**Rancher Desktop is not a requirement for this course.**

You can also use Docker Desktop or another compatible container/Kubernetes environment.

---

## Prerequisite

If you are using Windows, make sure WSL2 is installed first.

See:

[WSL2 Setup](wsl.md)

---

## Install Rancher Desktop

Download Rancher Desktop from the official documentation:

https://docs.rancherdesktop.io/getting-started/installation/

Follow the installation instructions for your operating system and the current Rancher Desktop version.

Installation steps can change between versions, so this course intentionally does not duplicate the detailed installation procedure here.

---

## Windows Setup

On Windows, Rancher Desktop uses Windows virtualization capabilities and WSL2.

A simplified view is:

```text
Windows
   |
   +-- WSL2
   |     |
   |     +-- Ubuntu
   |
   +-- Rancher Desktop
         |
         +-- Container Runtime
         |
         +-- Kubernetes
```

WSL2 and Rancher Desktop serve different purposes.

### WSL2

Provides the Linux environment and Linux kernel integration.

### Rancher Desktop

Provides the local container and Kubernetes development environment.

Therefore:

```text
WSL2 running
```

does not necessarily mean:

```text
Docker is available
```

Rancher Desktop also needs to be running when your Docker commands depend on its container runtime.

---

## Select the Container Runtime

Rancher Desktop supports different container runtimes.

For this course, use:

```text
Moby
```

The Moby runtime provides a Docker-compatible environment.

This allows us to use commands such as:

```powershell
docker build
```

```powershell
docker run
```

```powershell
docker ps
```

```powershell
docker images
```

Rancher Desktop's container-engine documentation:

https://docs.rancherdesktop.io/ui/preferences/container-engine/general/

---

## Docker CLI vs nerdctl

Rancher Desktop can work with different container runtimes and command-line tools.

A simplified view is:

```text
Moby
 |
 +-- Docker CLI
       docker build
       docker run
       docker ps


containerd
 |
 +-- nerdctl
       nerdctl build
       nerdctl run
       nerdctl ps
```

For this course, use:

```text
Moby + Docker CLI
```

You do not need to learn `nerdctl` to complete the course.

---

## Start Rancher Desktop

Start Rancher Desktop from Windows.

Wait until Rancher Desktop has finished starting before using Docker commands.

Then verify Docker:

```powershell
docker version
```

You should see information for both:

```text
Client
Server
```

The Server section confirms that the Docker-compatible container engine is available.

---

## Verify Docker

Run:

```powershell
docker version
```

Then:

```powershell
docker info
```

If both commands work, the Docker engine is available.

You can also run a simple test container:

```powershell
docker run --rm hello-world
```

A successful run should display a message indicating that Docker is working correctly.

The `--rm` option tells Docker to automatically remove the container after it exits.

---

## Verify kubectl

Run:

```powershell
kubectl version --client
```

This verifies that the Kubernetes command-line client is available.

---

## Enable Kubernetes

If you intend to use Rancher Desktop's local Kubernetes cluster, enable Kubernetes through the Rancher Desktop settings.

After Kubernetes has finished starting, run:

```powershell
kubectl get nodes
```

You should see at least one node with a `Ready` status.

Example:

```text
NAME              STATUS   ROLES
rancher-desktop   Ready    control-plane
```

The exact output can vary depending on the Rancher Desktop version.

---

## Verify Kubernetes

Run:

```powershell
kubectl cluster-info
```

Then:

```powershell
kubectl get nodes
```

The second command should show a node with:

```text
STATUS
Ready
```

At this point you have verified both major parts of the local environment:

```text
Docker
  |
  +-- Container Runtime


Kubernetes
  |
  +-- Local Cluster
```

---

## Rancher Desktop and WSL

After Rancher Desktop is installed, you may see additional WSL distributions.

For example:

```powershell
wsl -l -v
```

might show:

```text
  NAME                    STATE           VERSION
* Ubuntu                  Running         2
  rancher-desktop         Stopped         2
  rancher-desktop-data    Stopped         2
```

The exact output depends on your Rancher Desktop version and configuration.

The following distributions are managed by Rancher Desktop:

```text
rancher-desktop
rancher-desktop-data
```

You normally should **not** work directly inside these distributions.

Use your normal Ubuntu environment for Linux work.

---

## Important Concept

It is useful to distinguish these components:

| Component | Role |
|---|---|
| WSL2 | Linux environment |
| Ubuntu | Your Linux distribution |
| Rancher Desktop | Local container/Kubernetes development environment |
| Moby | Docker-compatible container engine |
| Docker CLI | Client used to interact with the container engine |
| Kubernetes | Container orchestration platform |
| kubectl | Kubernetes command-line client |

The relationship can be visualized as:

```text
Windows
   |
   +-- WSL2
   |     |
   |     +-- Ubuntu
   |
   +-- Rancher Desktop
         |
         +-- Moby
         |    |
         |    +-- Docker CLI
         |
         +-- Kubernetes
              |
              +-- kubectl
```

Understanding this distinction will become useful when we troubleshoot containers and Kubernetes later in the course.


## Official Documentation

Rancher Desktop:

https://docs.rancherdesktop.io/

Installation:

https://docs.rancherdesktop.io/getting-started/installation/

Container Engine:

https://docs.rancherdesktop.io/ui/preferences/container-engine/general/

Docker:

https://docs.docker.com/

Kubernetes:

https://kubernetes.io/docs/

---

## Next Step

Once your environment is verified, continue with:

[Module 01 – Docker Fundamentals](../../01-docker-fundamentals/README.md)