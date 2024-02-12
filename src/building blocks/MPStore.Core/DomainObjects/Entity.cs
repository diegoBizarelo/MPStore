using MPStore.Core.Messages;

namespace MPStore.Core.DomainObjects
{
    public class Entity
    {
        public Guid Id { get; set; }
        public Entity()
        {
            Id = Guid.NewGuid();
        }

        private List<Evento> _events;

        public IReadOnlyCollection<Evento> Notificacoes => _events.AsReadOnly();

        public void AdcionarEvento(Evento @event)
        {
            _events ??= new List<Evento>();
            _events.Add(@event);
        }

        public void RemoverEvento(Evento enventItem)
        {
            _events.Remove(enventItem);
        }

        public void LimparEventos()
        {
            _events.Clear();
        }

        public override string ToString()
        {
            return $"{GetType().Name} [Id={Id}]";
        }

        public override bool Equals(object obj)
        {
            var compareTo = obj as Entity;

            if (ReferenceEquals(this, compareTo)) return true;
            if (ReferenceEquals(null, compareTo)) return false;

            return Id.Equals(compareTo.Id);
        }

        public static bool operator ==(Entity a, Entity b)
        {
            if (a is null && b is null)
                return true;

            if (ReferenceEquals(a, null) || ReferenceEquals(b, null))
                return false;

            return a.Equals(b);
        }

        public static bool operator !=(Entity a, Entity b)
        {
            return !(a == b);
        }

        public override int GetHashCode()
        {
            return (GetType().GetHashCode() * 907) + Id.GetHashCode();
        }

        
    }
}
