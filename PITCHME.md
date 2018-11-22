# Advanced Docker

Developer Brown-bag Lunch

Nov 2018

---

### Agenda

- Persistent storage
- Service settings
- MyBoilerplate
- CI (Automated build and deployment)

---

### Persisted storage I – Options 

![](docs/docker-storage.png "Docker Storage")

- @size[.7em](Volumes - stored in a part of the host filesystem BUT managed by Docker)
- @size[.7em](Bind mount – “shared” folder from the host. Non-Docker processes on the host can modify them at any time)
- @size[.7em](Tmpfs - in the host system’s memory only)

---

### Persisted storage II – Available Shares

- @size[.7em](Using our SAMBA machines, we have shares on the docker hosts (docker1-4 and dockerl1-4))
- @size[.7em](3 different SAMBA shares:)
  - @size[.5em](_user_: Primary share for all productive use cases (docker services))
  - @size[.5em](_data_: Used generic for any kind of data (exports, backups, etc.))
  - @size[.5em](_system_: Used administratively (e.g. Postgres WAL for clustered DB))
- @size[.7em](In Windows nodes we have access to __U:\\__, which represents the ‘user’ share)
  - @size[.5em](Should be “bound” to __c:\data__ by default)
- @size[.7em](In Linux nodes we have access to __/data/user__, which represents the ‘user’ share)
  - @size[.5em](Should be “bound” to __/data__ by default)

---

### Persisted storage III – Usage 

- Each service gets it’s own folder under the user share
  - @size[.7em](/data/user/containers/\[service-name](-dev))
  - @size[.7em](U:\containers\\\[service-name](-dev))
- Bind mount can simply be defined in Portainer:

![](docs/portainer-mount.png "Portainer Mount")

@size[.7em](Bind /data/user/containers/tilecacheservice /data)

---

### Service settings I – Background

- @size[.7em](Some settings shouldn’t be present in source code)
  - @size[.7em](DB connection strings)
  - @size[.7em](API Keys)
  - @size[.7em](Any other passwords, etc.)
- @size[.7em](Or should be adjustable depending on the runtime environment)
  - @size[.7em](Logging endpoint)
  - @size[.7em](Connection string)
- @size[.7em](Inject them into the services using Environment Variables)
  - ```
	docker service create --env DEBUG=1 ...
	```
  - ```
	environment:
	  DEBUG=1
	```
	
---

### Service settings II - Suggestions

- @size[.7em](Use environment vars to override default appsettings)
  ```csharp
  config.AddJsonFile("appsettings.json", false, true);
  config.AddJsonFile($"appsettings.{env.EnvironmentName}.json", true, true);
  config.AddEnvironmentVariables();
	```

- @size[.7em](Use Options Pattern in ASP.NET Core, which supports DI)
  ```
  services.Configure<MyOptions>(Configuration);

  public DefaultController(ILogger<DefaultController> logger, 
      IOptions<ServiceOptions> serviceOptions, ...) 
  { 
  ```

---

### Service settings III – Portainer example

![](docs/portainer-environment.png "Portainer Environment")

---

### MyBoilerplate

#### [https://github.com/pergerch/MyBoilerplate](https://github.com/pergerch/MyBoilerplate)

- @size[.7em](ASP.NET Core 2.1 API)
- @size[.7em](Docker)
- @size[.7em](Settings (Appsettings, Environment variables, Options Pattern))
- @size[.7em](Automapper)
- @size[.7em](SEQ Logging)
- @size[.7em](EF Core (DbContext, Entities, Proxies, Repositories, Spatial Focus extension))
- @size[.7em](Stylecop + Analyzers)
- @size[.7em](Resharper config)

---

### Continuous Integration I

- Trigger the Azure pipeline on merging of pull requests
- Add build badge :cool:

![](docs/badge.png "Build Badge")

---

### Continuous Integration II

- Deactivate commits to master branch
  - Settings - Branch protection rules
- Setup Azure pipeline
  - docker-compose build
  - docker push
  - call Portainer webhook
