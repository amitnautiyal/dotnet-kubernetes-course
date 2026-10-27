# Environment Setup

This folder contains the environment setup guidance required to follow the .NET + Docker + Kubernetes course.

> **This is a reference module, not part of the 30-milestone course.**

The course focuses on learning Docker, containers, Kubernetes, and how to architect .NET applications for these environments. Installation and machine-specific configuration are intentionally kept outside the main learning sequence.

## Prerequisites

You need a local development environment capable of running:

- .NET SDK
- Docker-compatible container runtime
- Docker CLI
- Kubernetes
- kubectl

### Recommended Windows Setup

For Windows, one possible setup is:

```text
Windows
   |
   +-- WSL2
   |
   +-- Rancher Desktop
         |
         +-- Docker / Moby
         |
         +-- Kubernetes
```

You can also use Docker Desktop or another compatible container/Kubernetes environment.

**Rancher Desktop is not a requirement for this course.**

## If You Are Using Windows + WSL2 + Rancher Desktop

See:

- [WSL2 Setup](windows/wsl.md)
- [Rancher Desktop Setup](windows/rancher-desktop.md)
- [troubleshooting](windows/troubleshooting.md)

These documents are intended as quick-reference setup instructions rather than detailed tutorials.

For installation details that may change between software versions, use the official documentation linked from these files.

## Minimum Verification

Before starting course 01, make sure Docker is working:

```powershell
docker version
```

You should see both the Docker Client and Server information.

Verify the Docker environment:

```powershell
docker info
```

Verify kubectl:

```powershell
kubectl version --client
```

If you are using a local Kubernetes cluster, verify that Kubernetes is running:

```powershell
kubectl get nodes
```

You should see at least one Kubernetes node with a `Ready` status.

Example:

```text
NAME              STATUS   ROLES
rancher-desktop   Ready    control-plane
```

The exact output will vary depending on your environment and software versions.

## What Each Component Does

It is useful to understand the role of each component before starting the course.

| Component | Purpose |
|---|---|
| Windows | Host operating system |
| WSL2 | Linux environment and Linux kernel integration on Windows |
| Rancher Desktop | Local container and Kubernetes development environment |
| Docker / Moby | Container runtime and Docker-compatible engine |
| Docker CLI | Command-line interface for building and running containers |
| Kubernetes | Container orchestration platform |
| kubectl | Command-line client for Kubernetes |

We will learn the details of these technologies throughout the course.

You do not need to understand all of them before starting.

## Course Environment vs Course Concepts

The course is **not specific to Rancher Desktop**.

The concepts covered in the course apply to:

- Docker Desktop
- Rancher Desktop
- Azure Kubernetes Service (AKS)
- Amazon Elastic Kubernetes Service (EKS)
- Google Kubernetes Engine (GKE)
- Other Kubernetes environments

For example, the following are Kubernetes concepts rather than Rancher Desktop concepts:

```text
Pod
Deployment
Service
ConfigMap
Secret
Ingress
Health Probes
Resource Requests and Limits
Horizontal Pod Autoscaler
RBAC
NetworkPolicy
```

## Official Documentation

### Microsoft WSL

https://learn.microsoft.com/en-us/windows/wsl/install

https://learn.microsoft.com/en-us/windows/wsl/basic-commands

### Rancher Desktop

https://docs.rancherdesktop.io/

https://docs.rancherdesktop.io/getting-started/installation/

### Docker

https://docs.docker.com/

### Kubernetes

https://kubernetes.io/docs/

## Ready for Day 1?

If the following commands work:

```powershell
docker version
```

```powershell
kubectl version --client
```

and, when using local Kubernetes:

```powershell
kubectl get nodes
```

your environment is ready.

Move on to:

```text
01-docker-fundamentals
```