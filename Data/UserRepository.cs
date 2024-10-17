namespace DotNetApi.Data
{
    public class UserRepository : IUserRepository
    {
        DataContextEF _entityFramework;

        public UserRepository(IConfiguration config)
        {
            _entityFramework = new DataContextEF(config);
        }

        public bool SaveChanges()
        {
            return _entityFramework.SaveChanges() > 0;
        }

        public void AddEntity<T>(T entityToAdd)

        // public bool AddEntity<T>(T entityToAdd)
        {
            if(entityToAdd != null)
            {
                _entityFramework.Add(entityToAdd);
                // return true;
            }
            // return false;
        } 


        public void RemoveEntity<T>(T entityToAdd)

        // public bool AddEntity<T>(T entityToAdd)
        {
            if(entityToAdd != null)
            {
                _entityFramework.Remove(entityToAdd);
                // return true;
            }
            // return false;
        }         
    }
}