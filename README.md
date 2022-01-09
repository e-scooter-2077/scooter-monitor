# Scooter Monitor
[![Continuous Integration](https://github.com/e-scooter-2077/scooter-monitor/actions/workflows/ci.yml/badge.svg?event=push)](https://github.com/e-scooter-2077/scooter-monitor/actions/workflows/ci.yml)
[![GitHub issues](https://img.shields.io/github/issues-raw/e-scooter-2077/scooter-monitor?style=plastic)](https://github.com/e-scooter-2077/scooter-monitor/issues)
[![GitHub pull requests](https://img.shields.io/github/issues-pr-raw/e-scooter-2077/scooter-monitor?style=plastic)](https://github.com/e-scooter-2077/scooter-monitor/pulls)
[![GitHub](https://img.shields.io/github/license/e-scooter-2077/scooter-monitor?style=plastic)](/LICENSE)
[![GitHub release (latest SemVer including pre-releases)](https://img.shields.io/github/v/release/e-scooter-2077/scooter-monitor?include_prereleases&style=plastic)](https://github.com/e-scooter-2077/scooter-monitor/releases)
[![Documentation](https://img.shields.io/badge/domain%20model-click%20here-informational)](https://e-scooter-2077.github.io/documentation/domain-analysis/domain-models/e-scooter/scooter-control.html)

The Scooter Monitor service is resposible to publish events about Scooters status updates.

Scooter status updates come from the IoTHub service by Azure, in particular from Reported Properties updates of device twins.

Scooter Monitor is implemented as an Azure Function (FaaS) triggered by IoTHub that publishes events on the Service Bus topic shared by microservices of EScooter.
