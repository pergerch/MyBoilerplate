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

- Volumes - stored in a part of the host filesystem BUT managed by Docker
- Bind mount – “shared” folder from the host. Non-Docker processes on the host can modify them at any time
- Tmpfs - in the host system’s memory only
