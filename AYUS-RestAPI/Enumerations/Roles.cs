namespace AYUS_RestAPI.Enumerations
{
    public enum Roles
    {
        CLIENT = 0,
        MECHANIC = 1,
        ADMIN = 2,
    }

    public static class RoleParser
    {
        public static Roles GetRoles(this Roles roles, string role)
        {
            if(role.Equals("MECHANIC"))
                return Roles.MECHANIC;
            else if (role.Equals("ADMIN"))
                return Roles.ADMIN;
            return Roles.CLIENT;
        }
    }
}
