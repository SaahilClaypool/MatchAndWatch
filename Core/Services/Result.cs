using System;

// https://www.enterprise-software-development.eu/posts/2019/11/19/Option-type.html
namespace Core {

  public class Result<T> : Result<T, string> where T : notnull {
    public Result() : base(default, null) { }
    public Result(string error) : base(default, error) { }
    public Result(T value) : base(value, null) { }
  }

  public class Result<T, R> where T : notnull where R : notnull {

    public static Result<T, R> Fail(R? reason) => new Result<T, R>(default, reason);
    public static Result<T, R> Ok(T value) => new Result<T, R>(value, default);

    readonly bool _isOk;
    public readonly T? Value;
    readonly R? Error;

    public Result(T value) {
      Value = value;
      Error = default;
      _isOk = true;
    }

    public Result(R? error) {
      Error = error;
      Value = default;
      _isOk = false;
    }

    public Result(T? value, R? error) {
      Value = value ?? default;
      Error = error;
      _isOk = value is not null;
    }


    /// <summary>
    /// Returns true when IsSome, and sets the value
    /// <p>
    /// Usage: Result.IsOkay(out var value) ? onIsSome(value) : onIsNone();
    /// </p>
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public bool IsOk(out T value) {
      value = Value!;
      return _isOk;
    }

    /// <summary>
    /// Returns true when IsSome, and sets the value
    /// <p>
    /// Usage: Result.IsOkay(out var value) ? onIsSome(value) : onIsNone();
    /// </p>
    /// </summary>
    /// <param name="value"></param>
    /// <param name="error">assignment possible error</param>
    /// <returns></returns>
    public bool IsOk(out T value, out R error) {
      value = Value!;
      error = Error!;
      return _isOk;
    }

    /// <summary>
    /// Bind functions for when Some and None
    /// </summary>
    /// <param name="onIsSome">Run when is Some</param>
    /// <param name="onIsNone">Run when is None</param>
    /// <typeparam name="U">Result type</typeparam>
    /// <returns>Value or default defined by onIsNone</returns>
    public U Match<U>(
        Func<T, U> onIsOk,
        Func<R, U> onIsError) =>
            IsOk(out var value, out var error) ? onIsOk(value) : onIsError(error);

    /// <summary>
    /// Call the function when this is `Some`, else none
    /// </summary>
    /// <param name="binder">Operation when Some</param>
    /// <typeparam name="U">Result type</typeparam>
    /// <returns>Some(U) when Some, else None</returns>
    public Result<U, R> Bind<U>(Func<T, Result<U, R>> binder)
      where U : notnull =>
        Match(
            onIsOk: binder,
            onIsError: r => Result<U, R>.Fail(r));

    public Result<U, R> Map<U>(Func<T, U> mapper)
        where U : notnull =>
        Bind(
            value => Result<U, R>.Ok(mapper(value)));

    public Result<T, R> Filter(Predicate<T> predicate) {
      var This = this;
      return Bind(
          value => predicate(value) ? Result<T, R>.Ok(This.Value!) : Result<T, R>.Fail(default));
    }

    public T DefaultValue(T defaultValue) =>
        Match(
            onIsOk: value => value,
            onIsError: (_) => defaultValue);
  }
}
