// <copyright file="SlackIncomingWebHookHealthAlert.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using App.Metrics.Health;

namespace HealthSandboxMvc
{
    public class SlackIncomingWebHookHealthAlert : IReportHealthStatus
    {
        /// <inheritdoc />
        public async Task ReportAsync(HealthStatus status, CancellationToken cancellationToken = default)
        {
            var url = "https://hooks.slack.com/services/xxx";

            var slackMessage = new SlackHealthNotification
                               {
                                   Channel = "general"
                               };

            var unhealthyCheck = status.Results.Where(r => r.Check.Status == HealthCheckStatus.Unhealthy).ToList();

            if (unhealthyCheck.Any())
            {
                var attachment = new Attachment
                                 {
                                     Text = $"*{unhealthyCheck.Count}* checks failing.",
                                     Color = "#F35A00"
                                 };

                foreach (var result in unhealthyCheck)
                {
                    attachment.Fields.Add(new AttachmentFields
                                          {
                                              Title = result.Name,
                                              Value = $"_{result.Check.Message}_",
                                              Short = true
                                          });
                }

                slackMessage.Attachments.Add(attachment);
            }

            var degradedChecks = status.Results.Where(r => r.Check.Status == HealthCheckStatus.Degraded).ToList();

            if (degradedChecks.Any())
            {
                var attachment = new Attachment
                                 {
                                     Text = $"*{degradedChecks.Count}* checks degraded.",
                                     Color = "#FFCC00"
                                 };

                foreach (var result in degradedChecks)
                {
                    attachment.Fields.Add(new AttachmentFields
                                          {
                                              Title = result.Name,
                                              Value = $"_{result.Check.Message}_",
                                              Short = true
                                          });
                }

                slackMessage.Attachments.Add(attachment);
            }

            if (slackMessage.Attachments.Any())
            {
                using (var httpClient = new HttpClient())
                {
                    await httpClient.PostAsync(url, new JsonContent(slackMessage), cancellationToken);
                }
            }
        }
    }
}