# WSL2 Setup

## What is WSL2?

Windows Subsystem for Linux (WSL) allows you to run a Linux environment directly on Windows.

WSL2 uses a real Linux kernel and is commonly used as part of container development environments on Windows.

For this course, WSL2 is primarily an environment prerequisite.

---

## Do I Need WSL2?

If you are using **Windows with Rancher Desktop**, WSL2 is commonly used as part of the local container environment.

If WSL2 is already installed and working, you can skip the installation section.

> **macOS and Linux users do not need WSL2.** WSL2 is a Windows-specific technology. macOS and Linux users can proceed directly with their native container/Kubernetes environment, such as Rancher Desktop or Docker Desktop.

### In Short

| Platform | WSL2 Required? | Typical Setup |
|---|---|---|
| Windows | Recommended for this setup | Windows → WSL2 → Rancher Desktop |
| macOS | No | macOS → Rancher Desktop / Docker Desktop |
| Linux | No | Linux → Rancher Desktop / Docker / Kubernetes |

The goal of this course is to provide a consistent **Docker + Kubernetes** learning environment. The underlying operating system or desktop container product is not the focus of the course.

---

## Install WSL2

Open **PowerShell as Administrator** and run:

```powershell
wsl --install
```

Restart Windows if prompted.

The current Microsoft installation process enables the required Windows components, installs the Linux kernel, sets WSL2 as the default, and installs Ubuntu by default.

Official documentation:

https://learn.microsoft.com/en-us/windows/wsl/install

---

## Verify WSL Installation

Open PowerShell and run:

```powershell
wsl --status
```

This displays general WSL configuration information, including the default distribution and kernel version.

Then run:

```powershell
wsl -l -v
```

Example:

```text
  NAME      STATE           VERSION
* Ubuntu    Running         2
```

The important value is the `VERSION` column.

Your Linux distribution should be using:

```text
2
```

---

## Start WSL

You can start your default Linux distribution with:

```powershell
wsl
```

You should see a Linux shell similar to:

```text
amit@computer:~$
```

---

## Verify the Linux Environment

Inside the Linux shell, run:

```bash
whoami
```

This displays your Linux username.

Run:

```bash
pwd
```

This displays your current directory.

Run:

```bash
uname -a
```

This displays Linux kernel information.

For example:

```text
Linux computer 6.x.x-microsoft-standard-WSL2 ...
```

Exit the Linux shell with:

```bash
exit
```

---

## Useful WSL Commands

### List installed distributions

```powershell
wsl -l
```

### List distributions with their WSL versions

```powershell
wsl -l -v
```

### Show WSL status

```powershell
wsl --status
```

### Show WSL version information

```powershell
wsl --version
```

### Update WSL

```powershell
wsl --update
```

### Start WSL

```powershell
wsl
```

### Shut down WSL

```powershell
wsl --shutdown
```

`wsl --shutdown` stops all running WSL distributions and the WSL2 virtual machine.

### Start a specific distribution

```powershell
wsl -d Ubuntu
```

Replace `Ubuntu` with the name shown by:

```powershell
wsl -l
```

---

## Windows and Linux File Systems

WSL provides access to Windows drives.

For example, the Windows `C:` drive is normally available inside Linux as:

```text
/mnt/c
```

You can access it with:

```bash
cd /mnt/c
```

For this course, you do not need to move your entire development environment into WSL simply because you are using WSL2.

Your normal Windows development workflow can continue to be used.

---

## WSL and Rancher Desktop

It is important to understand that **WSL2 and Rancher Desktop are different things**.

A useful conceptual model is:

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

WSL2 provides the Linux environment.

Rancher Desktop provides the local container and Kubernetes development environment.

Therefore:

```text
WSL2 running
```

does **not** necessarily mean:

```text
Docker is available
```

Rancher Desktop must also be running when your Docker commands depend on its container runtime.

---

## WSL Distributions Managed by Rancher Desktop

After installing Rancher Desktop, you may see additional WSL distributions.

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

The Rancher Desktop distributions are managed by Rancher Desktop.

You normally should not work directly inside:

```text
rancher-desktop
rancher-desktop-data
```

Use your normal Ubuntu distribution for Linux work.

---

## Basic Troubleshooting

### WSL Is Not Installed

Run PowerShell as Administrator:

```powershell
wsl --install
```

Restart Windows if requested.

---

### Distribution Is Using WSL1

Check:

```powershell
wsl -l -v
```

If the distribution shows:

```text
VERSION
1
```

you can convert it with:

```powershell
wsl --set-version Ubuntu 2
```

---

### WSL Is Not Responding Correctly

Try:

```powershell
wsl --shutdown
```

Then start it again:

```powershell
wsl
```

---

### Need to Update WSL

Run:

```powershell
wsl --update
```

Then check:

```powershell
wsl --version
```

---

## Official Microsoft Documentation

WSL installation:

https://learn.microsoft.com/en-us/windows/wsl/install

WSL basic commands:

https://learn.microsoft.com/en-us/windows/wsl/basic-commands

WSL development environment:

https://learn.microsoft.com/en-us/windows/wsl/setup/environment

WSL overview:

https://learn.microsoft.com/en-us/windows/wsl/about

---

## Ready for the Course?

You are ready to continue when:

```powershell
wsl -l -v
```

shows your Linux distribution using:

```text
VERSION 2
```

For example:

```text
  NAME      STATE           VERSION
* Ubuntu    Running         2
```

You do not need to become a WSL expert for this course.

The learning path is:

```text
.NET
  |
Docker
  |
Containers
  |
Kubernetes
  |
AKS
  |
Cloud Architecture
```

## Next Step

If you are using Rancher Desktop, continue with:

[Rancher Desktop Setup](rancher-desktop.md)