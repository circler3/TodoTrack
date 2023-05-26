# TodoTrack
`TodoTrack` is a service which provides Todo management with time consumption logging.

## Usage
`TodoTrack` consists of several modules.
### Console
`TodoTrack` has a console service for Windows only. Run it will collect information of working status.

### WebAPI(no available yet)
Clone and open `sln` file. Set the api project as start project and run.

## Initial purpose
I like to use todo apps to manage my work and life. *Microsoft To-do* provides system-level integration and WebAPI interface. But it cannot track time. *shiguangxu* can track time consumption. However it lacks PC features. So I want to build a multi-platform Todo app which is capable of logging time.

## Core features
There are several core backlogs of `TodoTrack`:
[x] Foreground process based time consumption logging.
[ ] Automatically matching Todo items with working period.
[ ] Todo management.
[ ] Time consumption statistic.
[ ] Data Export feature.
[ ] Open data interface (OData).

## Backlogs
[x] Foreground process based time consumption logging.
[ ] Basic management

## Design
I use WebAPI to expose system service. `ABP vNext`, `OData`, `MongoDB` are used.

> Design may change over time.

## How to contribute
- Report bugs: Search current **issues** and open a new one if they can not solve your question.
- Push new features: Early PRs are recommended. If you have any ideas which need to be discussed, please turn to **Discussion**.

## **Help is appreciated**
I am not skilled in `FE` technologies. So currently I build a simple console app to use it. If you are interested in building FE part (Web/Mobile/PC/MAC), please start a PR of your own implementation. 

## Disclaimer
This project is currently alpha stage. Everything can change at anytime. Backup data!