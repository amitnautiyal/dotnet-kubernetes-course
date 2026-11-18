# Part 1: Environment Check, Images and Containers

> **Module:** 01 --- Container Fundamentals\
> **Part:** 01\
> **Focus:** Verify the local container environment and understand
> Docker images, containers, and their lifecycle.

## Learning objectives

By the end of this part, you should be able to:

-   Verify that WSL2, Rancher Desktop, Docker, kubectl, and .NET are
    available.
-   Distinguish a Docker image from a container.
-   Explain why one image can be used to create multiple containers.
-   Run a container in the foreground and in detached mode.
-   Publish a container port to the host.
-   Stop, start, inspect, execute commands in, view logs from, and
    remove a container.
-   Explain why removing a container does not necessarily remove its
    image.

## 1. Environment readiness

This course demonstrates container development on Windows using WSL2 and
Rancher Desktop. Module 01 begins with a readiness check; it does not
cover installation.

### 1.1 Verify WSL2

Run in **Windows PowerShell**:

``` powershell
wsl --list --verbose
```

**What it does**

Lists installed WSL distributions, their running state, and WSL version.

**Expected observation**

-   The Ubuntu distribution should use WSL version 2.
-   Rancher Desktop's distributions may also appear.
-   `rancher-desktop-data` being stopped is not, by itself, a problem.

**Observed in this environment**

-   `Ubuntu` --- Running, version 2.
-   `rancher-desktop` --- Running, version 2.
-   `rancher-desktop-data` --- Stopped, version 2.

### 1.2 Verify Rancher Desktop

Run in **Windows PowerShell**:

``` powershell
rdctl version
```

**What it does**

Reports the Rancher Desktop command-line client's version and the
targeted server version.

**Expected observation**

The command should complete and report the installed client version.

**Observed in this environment**

-   Rancher Desktop application: `1.25.0`.
-   `rdctl` client: `v1.25.0`.
-   Targeting server version: `v1`.

### 1.3 Verify Docker client and engine

Run in **Windows PowerShell**:

``` powershell
docker version
```

**What it does**

Displays both the Docker CLI (client) and the Docker Engine (server)
details. This is more useful than checking only the CLI because Docker
commands need a reachable engine.

**Expected observation**

The output should contain both a **Client** section and a **Server**
section. In this Rancher Desktop setup, the client runs on Windows while
the engine runs in the Linux container environment.

**Observed in this environment**

  | Component | Observed version/details |
  | --- | ---|
  | Docker CLI | `29.9.3-rd` | 
  | Client OS/architecture | `windows/amd64` | 
  | Docker Engine | `29.9.2` | 
  | Engine OS/architecture | `linux/amd64` | 
  | Docker API | `1.56` | 
  | containerd | `2.3.7` | 
  | runc | `1.4.3` | 

The client and engine patch versions are not identical. That is not, by
itself, an error; the important readiness signal is that the command can
communicate with the server and display its details.

### 1.4 Verify kubectl

Run in **Windows PowerShell**:

``` powershell
kubectl version --client
```

**What it does**

Reports the locally installed Kubernetes command-line client. The
`--client` option deliberately checks the client only; it does not
verify a Kubernetes cluster connection or server version.

**Expected observation**

The client version and Kustomize version should be displayed.

**Observed in this environment**

-   kubectl client: `v1.36.5`.
-   Kustomize: `v5.8.1`.

### 1.5 Verify the .NET SDK and runtimes

Run in **Windows PowerShell**:

``` powershell
dotnet --version
dotnet --list-runtimes
```

**What they do**

-   `dotnet --version` reports the selected .NET SDK.
-   `dotnet --list-runtimes` lists the installed shared runtimes.

**Expected observation**

The .NET 10 SDK and compatible .NET/ASP.NET Core runtimes should be
available for the course's .NET 10 application.

**Observed in this environment**

-   .NET SDK: `10.0.404`.
-   `Microsoft.NETCore.App`: `10.0.15` (along with earlier installed
    runtime versions).
-   `Microsoft.AspNetCore.App`: `10.0.15` (along with earlier installed
    runtime versions).

### 1.6 Readiness snapshot

The following table records the versions actually observed during this
environment check. These are environment records, not permanent course
requirements.

  | Component | Observed version |
  | --- | --- | 
  | Rancher Desktop | `1.25.0` | 
  | rdctl | `1.25.0` | 
  | Docker CLI | `29.9.3-rd` | 
  | Docker Engine | `29.9.2` | 
  | Docker API | `1.56` | 
  | containerd | `2.3.7` | 
  | runc | `1.4.3` | 
  | kubectl client | `v1.36.5` | 
  | Kustomize | `v5.8.1` | 
  | .NET SDK | `10.0.404` | 
  | .NET and ASP.NET Core 10 runtimes | `10.0.15` | 

**Verification boundary:** The kubectl command above checked the client
only. The Kubernetes cluster/server version was not verified as part of
this check.

## 2. Inspect existing Docker resources before experimenting

Before creating course resources, inspect what is already present.

Run in **Windows PowerShell**:

``` powershell
docker ps -a
docker images
docker network ls
docker volume ls
```

**What the commands do**

  | Command | Purpose |
  | --- | --- |
  | `docker ps -a` | Lists running and stopped containers. |
  | `docker images` | Lists locally available images. |
  | `docker network ls` | Lists Docker networks. |
  | `docker volume ls` |  Lists Docker volumes. |

**Expected observation**

A new or lightly used environment may still contain resources created by
Rancher Desktop or its Kubernetes distribution. Do not assume every
listed container or image belongs to a previous exercise.

**Observed in this environment**

-   Rancher Desktop/Kubernetes-managed containers were present,
    including components for Traefik, CoreDNS, metrics-server, and
    local-path-provisioner.
-   Rancher-related images were present.
-   Docker networks listed: `bridge`, `host`, and `none`.
-   No Docker volumes were listed.

**Important:** Do not indiscriminately remove Rancher Desktop or
Kubernetes-managed resources. In particular, do not use broad prune
commands such as `docker system prune` as a routine cleanup step in this
environment. Identify ownership before removing anything. No cleanup of
the Rancher-managed resources was required for this exercise.

## 3. Docker in a nutshell

Docker provides tools and an engine for building, distributing, and
running applications in containers. A container packages an application
process with the filesystem content and configuration it needs, while
using the host's operating-system kernel rather than booting a separate
guest operating system for every container.

### 3.1 Image versus container

An **image** is an immutable, versioned template used to create
containers. It contains the filesystem layers and metadata needed to
start the container.

A **container** is a created, runnable instance of an image. It has its
own lifecycle and a writable layer associated with that container.

One image can be used to create multiple containers. Those containers
share the image's read-only layers but have separate runtime state and
writable layers.

### 3.2 A simplified view of this local environment

For this course's Windows setup, the high-level relationship is:

``` text
Windows
  |
  +-- WSL2
        |
        +-- Rancher Desktop
              |
              +-- Docker-compatible CLI / Docker Engine
                    |
                    +-- Linux containers
```

This is a conceptual view, not a complete implementation diagram. The
Docker CLI sends requests to the Docker Engine API. In the Linux
container environment, containerd and runc participate in managing and
starting containers. Linux containers use a Linux kernel; they are not
separate Windows or Linux virtual machines.

### 3.3 Why containers are relatively lightweight

Containers share the host kernel. Linux kernel features such as
namespaces and control groups help isolate processes and manage resource
usage. Images are assembled from layers, and a container adds its own
writable layer on top of the image layers.

A virtual machine normally runs its own guest operating system and
kernel. A container does not boot a separate kernel for each container.
This is an important distinction, but it does not mean containers
provide the same isolation boundary as a dedicated virtual machine.

## 4. Run the first container

We will use the small `hello-world` image to observe the basic
image-to-container flow.

Run in **Windows PowerShell**:

``` powershell
docker run --name module01-hello-world hello-world
```

**What the command means**

-   `docker run` creates and starts a container.
-   `--name module01-hello-world` assigns the container a readable name.
-   `hello-world` is the image reference. If the image is not present
    locally, Docker attempts to pull it before creating the container.

**Expected observation**

-   Docker pulls the image if it is not already available locally.
-   The container prints a short success message: `Hello from Docker!`.
-   The process exits, so the container stops.

**Observed in this environment**

The image was not present locally, Docker pulled `hello-world:latest`,
and the program printed:

``` text
Hello from Docker!
```

The container completed successfully.

### 4.1 Confirm the container and image

Run in **Windows PowerShell**:

``` powershell
docker ps -a --filter "name=module01-hello-world"
docker image ls hello-world
```

**What the commands do**

-   The first command lists containers, including stopped containers,
    filtered by name.
-   The second lists the locally available `hello-world` image.

**Expected observation**

The container should appear with an exited status, while the image
remains available locally.

**Observed in this environment**

-   Container name: `module01-hello-world`.
-   Container ID prefix: `20d68abb6f80`.
-   Status: `Exited (0)`.
-   Image: `hello-world:latest`.
-   Image ID prefix: `1e4c05b897a7`.
-   Reported image disk usage: `25.9kB`; reported content size:
    `9.49kB`.

Exit code `0` indicates that the container's main process completed
successfully.

## 5. One image, multiple containers

Run the same image again, assigning a different container name.

Run in **Windows PowerShell**:

``` powershell
docker run --name module01-hello-world-2 hello-world
docker ps -a --filter "ancestor=hello-world"
```

**What the commands do**

-   The first command creates and runs another container from the
    already downloaded image.
-   The second lists containers created from the `hello-world` image.

**Expected observation**

Two separate containers should be listed, both using the same image and
both exited successfully.

**Observed in this environment**

  | Container | Container ID prefix | Status |
  | --- |--- |
  | `module01-hello-world-2` | `133c9d5a1b94` | `Exited (0)` |
  | `module01-hello-world` | `20d68abb6f80 ` | `Exited (0)` |

**Key takeaway:** An image is not a running container. One image can be
used to create many containers. Each container has its own identity and
lifecycle.

## 6. Inspect container state

Run in **Windows PowerShell**:

``` powershell
docker inspect --format '{{.State.Status}} | ExitCode={{.State.ExitCode}} | Image={{.Config.Image}}' module01-hello-world
```

**What the command means**

-   `docker inspect` retrieves detailed metadata about a Docker object.
-   `--format` selects specific fields rather than printing the full
    JSON document.
-   `.State.Status` reports the container state.
-   `.State.ExitCode` reports the process exit code.
-   `.Config.Image` reports the image reference used to create the
    container.

**Expected observation**

The output should show that the container exited, its exit code, and the
image reference.

**Observed in this environment**

``` text
exited | ExitCode=0 | Image=hello-world
```

Inspection is useful for troubleshooting because it exposes the actual
configuration and state Docker has recorded, rather than relying only on
assumptions.

## 7. Run a long-lived container in detached mode

The `hello-world` process exits after printing its message. To explore a
container that remains running, use Nginx.

Run in **Windows PowerShell**:

``` powershell
docker run -d --name module01-nginx -p 8080:80 nginx
```

**What the command means**

-   `docker run` creates and starts a container.
-   `-d` runs it in detached mode, in the background.
-   `--name module01-nginx` assigns a readable name.
-   `-p 8080:80` publishes host port `8080` and forwards traffic to
    container port `80`.
-   `nginx` is the image reference.

The port syntax is:

``` text
-p <host-port>:<container-port>
```

**Expected observation**

-   Docker downloads the Nginx image if it is not already local.
-   Docker prints a container ID.
-   The container appears as running in `docker ps`.
-   Opening `http://localhost:8080` on the Windows host displays the
    Nginx welcome page.

**Observed in this environment**

The Nginx container started and appeared in `docker ps`. Opening
`http://localhost:8080` displayed the Nginx welcome page.

**Key takeaway:** The service listens on port `80` inside the container.
The published host port `8080` makes it reachable through the host's
`localhost:8080`.

## 8. Stop and start a container

Run in **Windows PowerShell**:

``` powershell
docker stop module01-nginx
docker ps -a
docker start module01-nginx
```

**What the commands do**

-   `docker stop` asks the container's main process to stop.
-   `docker ps -a` lists containers, including stopped ones.
-   `docker start` starts an existing stopped container. It does not
    create a new container.

**Expected observation**

-   After `docker stop`, the container should still exist but show an
    exited status.
-   After `docker start`, the same container should be running again.
-   The Nginx page should again be reachable at `http://localhost:8080`.

**Observed in this environment**

The container changed to `Exited (0)` after the stop command. Starting
it brought the same container back to a running state; its container ID
remained the same, and the URL worked again.

**Key takeaway:** Stopping a container does not remove it. Starting it
reuses the existing container and its configuration.

## 9. View container logs

Run in **Windows PowerShell**:

``` powershell
docker logs --tail 10 module01-nginx
```

**What the command means**

-   `docker logs` displays output written by the container's main
    process to standard output and standard error.
-   `--tail 10` limits the output to the most recent ten log lines.

Other useful forms include:

``` powershell
docker logs module01-nginx
docker logs -f module01-nginx
docker logs --since 5m module01-nginx
```

-   The first displays available logs.
-   `-f` follows new log output.
-   `--since 5m` limits output to logs from approximately the last five
    minutes.

**Expected observation**

Nginx startup messages and HTTP request log entries should appear after
the page has been requested.

**Observed in this environment**

The logs included Nginx worker startup notices, a successful browser
request for `/` with HTTP status `200`, and a request for `/favicon.ico`
with HTTP status `404`.

The favicon response did not mean that the Nginx welcome page had
failed. The main page request returned `200`; the browser separately
requested a favicon that was not present.

PowerShell displayed some stderr output using its `NativeCommandError`
presentation. That display alone did not indicate that Docker had
failed.

## 10. Inspect a running container

Run in **Windows PowerShell**:

``` powershell
docker inspect --format '{{.State.Status}}' module01-nginx
docker inspect --format '{{.NetworkSettings.Ports}}' module01-nginx
docker inspect --format '{{.Config.Image}}' module01-nginx
```

**What the commands do**

They inspect selected fields from the container's recorded state, port
configuration, and image configuration.

**Expected observation**

-   The state should be `running`.
-   The port mapping should show container port `80/tcp` published on
    host port `8080`.
-   The image reference should be `nginx`.

**Observed in this environment**

``` text
running
map[80/tcp:[{0.0.0.0 8080} {:: 8080}]]
nginx
```

The port output shows bindings for IPv4 (`0.0.0.0`) and IPv6 (`::`) on
host port `8080`.

## 11. Execute a command inside a running container

`docker exec` starts an additional process inside an already running
container. It does not create another container.

### 11.1 Run individual commands

Run in **Windows PowerShell**:

``` powershell
docker exec module01-nginx hostname
docker exec module01-nginx nginx -v
```

**What the commands mean**

-   `docker exec` runs a command in an existing running container.
-   `module01-nginx` identifies the target container.
-   The remaining argument is the command to execute inside it.

**Expected observation**

The first command prints the container hostname. The second prints the
Nginx version.

**Observed in this environment**

-   Hostname: `15fe7c4ba429`.
-   Nginx version: `nginx/1.31.8`.

### 11.2 Open an interactive shell

Run in **Windows PowerShell**:

``` powershell
docker exec -it module01-nginx sh
```

**What the options mean**

-   `-i` keeps standard input open.
-   `-t` allocates a pseudo-terminal.
-   `sh` starts a shell inside the container.

The prompt and terminal behavior can vary by terminal host. If the shell
appears to be waiting without a visible prompt, try entering a command
or use another terminal window to continue.

**Commands run inside the container shell**

The following commands are not Windows PowerShell commands; they run
after entering the container shell:

``` sh
hostname
pwd
ls -la /usr/share/nginx/html
cat /etc/os-release
```

**Expected observation**

-   `hostname` prints the container hostname.
-   `pwd` prints the current directory.
-   `ls` lists the files served by the Nginx default site.
-   `cat /etc/os-release` identifies the container's Linux distribution.

**Observed in this environment**

-   Hostname: `15fe7c4ba429`.
-   Working directory: `/`.
-   The Nginx document directory contained `50x.html` and `index.html`.
-   The container identified itself as Debian GNU/Linux 13 (`trixie`),
    version `13.7`.

**Key takeaway:** A container is not a virtual machine with a full
interactive desktop. It runs processes, and `docker exec` lets an
operator start an additional process---such as a shell---for inspection
and troubleshooting.

## 12. Remove a container without removing its image

Run in **Windows PowerShell**:

``` powershell
docker stop module01-nginx
docker rm module01-nginx
docker ps -a --filter "name=module01-nginx"
docker image ls nginx
```

**What the commands do**

-   `docker stop` stops the running container.
-   `docker rm` removes the stopped container.
-   `docker ps -a` verifies whether the named container remains.
-   `docker image ls nginx` checks whether the image remains locally
    available.

**Expected observation**

-   The container should no longer appear in the container listing.
-   The Nginx image should still be present.

**Observed in this environment**

The Nginx container was removed, and the Nginx image remained available
locally.

**Key takeaway:** Containers and images are separate Docker objects.
Removing a container does not automatically remove the image from which
it was created.

## 13. Part 1 recap

-   WSL2, Rancher Desktop, Docker, kubectl, and .NET were checked before
    beginning the exercises.
-   The Docker CLI and Docker Engine are separate components;
    `docker version` reports both.
-   An image is a template; a container is an instance created from an
    image.
-   Multiple containers can be created from one image.
-   A container can exit successfully and still remain as a stopped
    container.
-   Detached mode runs a container in the background.
-   Port publishing maps a host port to a container port.
-   `stop`, `start`, and `rm` have different effects on the container
    lifecycle.
-   `logs`, `inspect`, and `exec` are useful operational troubleshooting
    tools.
-   Removing a container does not necessarily remove its image.

### Common wrong mental model

> **A container is a lightweight virtual machine that keeps its state.**

A container is better understood as an isolated process (or set of
processes) using the host kernel, with a filesystem view assembled from
image layers and a container-specific writable layer. A container's
writable layer is tied to that container; removing the container removes
that layer. Persistent data requires an explicit storage strategy, which
is covered later in this module.

## 14. Checkpoint questions

Try answering these before moving on:

1.  What is the difference between a Docker image and a container?
2.  Can two containers created from the same image have different
    lifecycles?
3.  What does `docker run -d -p 8080:80 nginx` do?
4.  What is the difference between `docker stop` and `docker rm`?
5.  What information can `docker inspect` provide that `docker ps` does
    not?
6.  Does `docker exec` create a new container?
7.  Why did the Nginx container continue to work after it was started
    again?
8.  Why did removing `module01-nginx` not remove the Nginx image?

## 15. Resource state at the end of Part 1

The following state reflects what was reported after the exercises:

  -----------------------------------------------------------------------
  Resource                            End state
  ----------------------------------- -----------------------------------
  `module01-nginx` container          Stopped and removed

  `nginx` image                       Retained locally

  `module01-hello-world` container    Stopped; not reported as removed

  `module01-hello-world-2` container  Stopped; not reported as removed

  `hello-world` image                 Retained locally                           

  `Rancher Desktop/Kubernetes-managed resources`  Left intact
                             
  -----------------------------------------------------------------------

Do not use broad prune commands to clean up the environment. If you
choose to remove the two stopped `hello-world` exercise containers,
remove only those named containers:

``` powershell
docker rm module01-hello-world module01-hello-world-2
```

This optional cleanup does not remove the `hello-world` image.

------------------------------------------------------------------------

**Next part:** Part 2 --- First Dockerfile and Port Mapping. We will use
the `Orders.Api` baseline and create the first application image.
