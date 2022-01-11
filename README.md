# Scooter Monitor
[![Continuous Integration](https://github.com/e-scooter-2077/scooter-monitor/actions/workflows/ci.yml/badge.svg?event=push)](https://github.com/e-scooter-2077/scooter-monitor/actions/workflows/ci.yml)
[![GitHub issues](https://img.shields.io/github/issues-raw/e-scooter-2077/scooter-monitor?style=plastic)](https://github.com/e-scooter-2077/scooter-monitor/issues)
[![GitHub pull requests](https://img.shields.io/github/issues-pr-raw/e-scooter-2077/scooter-monitor?style=plastic)](https://github.com/e-scooter-2077/scooter-monitor/pulls)
[![GitHub](https://img.shields.io/github/license/e-scooter-2077/scooter-monitor?style=plastic)](/LICENSE)
[![GitHub release (latest SemVer including pre-releases)](https://img.shields.io/github/v/release/e-scooter-2077/scooter-monitor?include_prereleases&style=plastic)](https://github.com/e-scooter-2077/scooter-monitor/releases)
[![Documentation](https://img.shields.io/badge/domain%20model-click%20here-informational?style=plastic)](https://e-scooter-2077.github.io/documentation/domain-analysis/domain-models/e-scooter/scooter-control.html)

The Scooter Monitor service is responsible of publishing Scooters status updates on the Service Bus.

Scooter status updates come from the IoTHub service by Azure, in particular from Reported Properties updates of each device twin.

Scooter Monitor is implemented as an Azure Function (FaaS) triggered by IoTHub. This function publishes events on the Service Bus topic shared by most microservices of EScooter.
