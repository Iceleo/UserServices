// https://bool.dev/blog/detail/spetsifikatsiya-pattern-proektirovaniya
// Классическая реализация спецификации
using System;

namespace Patterns.SpecificationClassic
{
    public abstract class SpecificationClassic<T> : ISpecification<T>
    {
        public abstract bool IsSatisfiedBy(T candidate);
        public ISpecification<T> And(ISpecification<T> other) => new AndSpecification<T>(this, other);
        public ISpecification<T> AndNot(ISpecification<T> other) => new AndNotSpecification<T>(this, other);
        public ISpecification<T> Or(ISpecification<T> other) => new OrSpecification<T>(this, other);
        public ISpecification<T> OrNot(ISpecification<T> other) => new OrNotSpecification<T>(this, other);
        public ISpecification<T> Xor(ISpecification<T> other) => new XorSpecification<T>(this, other);
        public ISpecification<T> Not() => new NotSpecification<T>(this);

        public static ISpecification<T> operator |(SpecificationClassic<T> firstCommand, ISpecification<T> secondCommand)
        {
            return new OrSpecification<T>(firstCommand, secondCommand);
        }

    }
    /// <summary>
    /// Выполняет логическое   над спецификациями
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class AndSpecification<T> : SpecificationClassic<T>
    {
        readonly ISpecification<T> _left;
        readonly ISpecification<T> _right;

        public AndSpecification(ISpecification<T> left, ISpecification<T> right)
        {
            _left = left;
            _right = right;
        }

        public override bool IsSatisfiedBy(T candidate) => _left.IsSatisfiedBy(candidate) && _right.IsSatisfiedBy(candidate);
    }
    /// <summary>
    /// Выполняет логическое  AND над спецификациями
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class AndNotSpecification<T> : SpecificationClassic<T>
    {
        readonly ISpecification<T> _left;
        readonly ISpecification<T> _right;

        public AndNotSpecification(ISpecification<T> left, ISpecification<T> right)
        {
            this._left = left;
            this._right = right;
        }

        public override bool IsSatisfiedBy(T candidate) => _left.IsSatisfiedBy(candidate) && (!_right.IsSatisfiedBy(candidate));
    }

    /// <summary>
    /// Выполняет логическое  OR над спецификациями
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class OrSpecification<T> : SpecificationClassic<T>
    {
        readonly ISpecification<T> _left;
        readonly ISpecification<T> _right;

        public OrSpecification(ISpecification<T> left, ISpecification<T> right)
        {
            _left = left;
            _right = right;
        }

        public override bool IsSatisfiedBy(T candidate) => _left.IsSatisfiedBy(candidate) || _right.IsSatisfiedBy(candidate);
    }

    /// <summary>
    /// Выполняет логическое   над спецификациями
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class OrNotSpecification<T> : SpecificationClassic<T>
    {
        readonly ISpecification<T> _left;
        readonly ISpecification<T> _right;

        public OrNotSpecification(ISpecification<T> left, ISpecification<T> right)
        {
            _left = left; _right = right;
        }

        public override bool IsSatisfiedBy(T candidate) => _left.IsSatisfiedBy(candidate) || !_right.IsSatisfiedBy(candidate);
    }

    /// <summary>
    /// Выполняет логическое  XOR над спецификациями
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class XorSpecification<T> : SpecificationClassic<T>
    {
        readonly ISpecification<T> _left;
        readonly ISpecification<T> _right;

        public XorSpecification(ISpecification<T> left, ISpecification<T> right)
        {
            _left = left;
            _right = right;
        }

        public override bool IsSatisfiedBy(T candidate) => _left.IsSatisfiedBy(candidate) ^ _right.IsSatisfiedBy(candidate);
    }

    /// <summary>
    /// Выполняет логическое NOT над спецификацией
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class NotSpecification<T> : SpecificationClassic<T>
    {
        readonly ISpecification<T> _other;
        public NotSpecification(ISpecification<T> other) => _other = other;

        public override bool IsSatisfiedBy(T candidate) => !_other.IsSatisfiedBy(candidate);
    }
}