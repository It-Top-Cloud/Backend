namespace cloud.Enums {
    public enum RolesEnum {
        User = 0b_0000_0001,
        Unlimited = 0b_0000_0010 | User,
        Admin = 0b_0000_0100 | Unlimited
    }
}
