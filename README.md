# TodoTrack
`TodoTrack` provides Todo management with automatic time consumption logging via foreground process analysis.

## Summary
`TodoTrack` consists of several modules. 
### Foreground time track (alpha)
`TodoTrack` has a console service for Windows only. It logs the application you are using in foreground. 

Run it will collect information of working status. SQLite is used for alpha stage. Make sure the data destination is 100% trusted.

### TodoTrack CLI (alpha)
A console cli tool can help deal with simple todo task management. It is used when GUI apps are not ready.

### WebAPI (not available yet)
Core service handles all Todo items and work time units. 

Clone and open `sln` file. Set the api project as start project and run.

### GUI apps with Pomodoro support (not available yet)
Offers Pomodoro approach which deal with fixed period work unit. It can help with scheduled work and rest time for healthier habits. However system does not collect Pomodoro statistics currently.

Run it will open a windows prompts for Todo items and work time unit. Start and enjoy.

## Initial purpose
I like to use todo apps to manage my work and life. *Microsoft To-do* provides system-level integration and WebAPI interface. But it cannot track time. `*shiguangxu*` can track time consumption. However it lacks PC features. So I want to build a multi-platform Todo app which is capable of logging time.

## Core features
There are several core backlogs of `TodoTrack`:
- [x] Foreground process based time consumption logging.
- [ ] Automatically matching Todo items with working period.
- [ ] Todo management.
- [ ] Open data interface (OData).
- [ ] Time consumption statistic.
- [ ] Data Export feature.

## Backlogs
- [x] Foreground process based time consumption logging.
- [ ] Management Console in Cli style
- [ ] Basic Todo management

## Design
I use WebAPI to expose system service. `ABP vNext`, `OData`, `MongoDB` are used. Currently console and window application support Windows only.

> Design may change over time.

## How to contribute
- Report bugs: Search current **issues** and open a new one if they can not solve your question.
- Push new features: Early PRs are recommended. If you have any ideas which need to be discussed, please turn to **Discussion**.

## **Help is appreciated**
I am not skilled in `FE` technologies. So currently I build a simple console app to use it. If you are interested in building FE part (Web/Mobile/PC/MAC), please start a PR of your own implementation. 

## Disclaimer
This project is currently at alpha stage. Anything can change at anytime. Backup data often and write migration code to help you with breaking changes.


