namespace KeyValueBase.Interfaces {
    public interface IPredicate<T> {
        bool Evaluate(T input);
    }
}
