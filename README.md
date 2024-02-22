# OTEL Proof of Concept for dotnet with the grafana stack

[![Licence](https://img.shields.io/badge/License-MIT-blue.svg)](LICENSE)

This repository contains a proof of concept for using OpenTelemetry with the Grafana stack (Grafana, Grafana Agent, Loki, Tempo and Prometheus) in dotnet 8.

## Features

- Logs, traces and metrics with native OpenTelemetry primitives
- Grafana Agent as central sidecar to the dotnet application
- Grafana Agent configuration to send logs, traces and metrics with the flow mode in OTLP

## License

This project is licensed under the MIT License.
