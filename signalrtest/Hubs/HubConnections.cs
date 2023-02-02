namespace signalrtest.Hubs
{
    public static class HubConnections
    {
        public static Dictionary<string, List<string>> Users = new();

        public static bool HasUserConnection( string userId, string connectionId )
        {
            try
            {
                if(Users.ContainsKey( userId ))
                {
                    return Users[userId].Any(x => x.Contains(connectionId));
                }
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
            }
            return false;
        }

        public static bool HasUser(string userId)
        {
            try
            {
                if (Users.ContainsKey(userId))
                {
                    return Users[userId].Any();
                }
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
            }
            return false;
        }

        public static void AdduserConnection(string userId, string connectionId)
        {
            if(!string.IsNullOrEmpty(userId) && !HasUserConnection(userId, connectionId))
            {
                if (Users.ContainsKey(userId))
                    Users[userId].Add(connectionId);
                else
                    Users.Add(userId,new List<string> {connectionId});
            }
            
        }

        public static List<string> OnlineUsers()
        {
            return Users.Keys.ToList();
        }

    }
}
