﻿namespace Anderson.PackageAudit.Users
{
    public class EnrolUserRequest
    {
        public string Username { get; set; }
        public string TenantName { get; set; }
        public bool OptInToMarketing { get; set; }
    }
}