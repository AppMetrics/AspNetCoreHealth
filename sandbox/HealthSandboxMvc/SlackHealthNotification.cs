// <copyright file="SlackHealthNotification.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System.Collections.Generic;

namespace HealthSandboxMvc
{
    public class SlackHealthNotification
    {
        public SlackHealthNotification()
        {
            Attachments = new List<Attachment>();
        }

        public string Text { get; set; }

        public string Channel { get; set; }

        public string UserName { get; set; }

        public string IconEmoji { get; set; }

        public List<Attachment> Attachments { get; set; }
    }

#pragma warning disable SA1402 // File may only contain a single class
    public class Attachment
    {
        public Attachment()
        {
            Fields = new List<AttachmentFields>();
        }

        public string Fallback { get; set; }

        public string Color { get; set; }

        public string PreText { get; set; }

        public string AuthorName { get; set; }

        public string AuthorLink { get; set; }

        public string AuthorIcon { get; set; }

        public string Title { get; set; }

        public string TitleLink { get; set; }

        public string Text { get; set; }

        public List<AttachmentFields> Fields { get; set; }
    }

    public class AttachmentFields
    {
        public string Title { get; set; }

        public string Value { get; set; }

        public bool Short { get; set; }

        public string ImageUrl { get; set; }

        public string ThumbUrl { get; set; }

        public string Footer { get; set; }

        public string FooterIcon { get; set; }
    }
#pragma warning restore SA1402 // File may only contain a single class
}