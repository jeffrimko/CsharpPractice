using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using FluentNHibernate.Mapping;
using NHibernate;
using NHibernate.Tool.hbm2ddl;
using System.IO;

namespace SqliteNhibernate
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var session = NHiberateSession.Open())
            using (var transaction = session.BeginTransaction())
            {

                session.Save(new Character
                {
                    Name = "Tuffy",
                    HitPoints = 20,
                    Armor = new Armor
                    {
                        Name = "Dragon Hide",
                        Rating = 4
                    },
                    Weapon = new Weapon
                    {
                        Name = "Flawless Sword",
                        Damage = 6
                    }
                });

                session.Save(new Character
                {
                    Name = "Nerfy",
                    HitPoints = 10,
                    Weapon = new Weapon
                    {
                        Name = "Dull Knife",
                        Damage = 2
                    }
                });

                var list = session
                    .QueryOver<Character>()
                    .Select(c => c.Name)
                    .OrderBy(c => c.Name).Asc
                    .List<string>();

                transaction.Commit();
            }
        }
    }

    class NHiberateSession
    {
        private const string DBPATH = "database.sqlite";
        private static ISessionFactory _sessionFactory;
        private static void Initialize()
        {
            _sessionFactory = Fluently
                .Configure()
                .Database(SQLiteConfiguration.Standard.UsingFile(DBPATH))
                .Mappings(m => m.FluentMappings
                    .Add<CharacterMap>()
                    .Add<WeaponMap>()
                    .Add<ArmorMap>())
                .ExposeConfiguration(c =>
                {
                    var dropCreateTables = !File.Exists(DBPATH);
                    var schemaExport = new SchemaExport(c);
                    schemaExport.Execute(true, dropCreateTables, false);
                })
                .BuildSessionFactory();
        }

        public static ISession Open()
        {
            if (_sessionFactory == null)
            {
                Initialize();
            }
            return _sessionFactory.OpenSession();
        }
    }

    class Weapon
    {
        public virtual int Id { get; set; }
        public virtual string Name { get; set; }
        public virtual int Damage { get; set; }
    }

    class Armor
    {
        public virtual int Id { get; set; }
        public virtual string Name { get; set; }
        public virtual int Rating { get; set; }
    }

    class Character
    {
        public virtual int Id { get; set; }
        public virtual string Name { get; set; }
        public virtual int HitPoints { get; set; }
        public virtual Weapon Weapon { get; set; }
        public virtual Armor Armor { get; set; }
    }

    class WeaponMap : ClassMap<Weapon>
    {
        public WeaponMap()
        {
            Id(x => x.Id);
            Map(x => x.Name);
            Map(x => x.Damage);
        }
    }

    class ArmorMap : ClassMap<Armor>
    {
        public ArmorMap()
        {
            Id(x => x.Id);
            Map(x => x.Name);
            Map(x => x.Rating);
        }
    }

    class CharacterMap : ClassMap<Character>
    {
        public CharacterMap()
        {
            Id(x => x.Id);
            Map(x => x.Name);
            Map(x => x.HitPoints);
            References(x => x.Weapon).Cascade.All();
            References(x => x.Armor).Cascade.All();
        }
    }
}
