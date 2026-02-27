import { useState } from "react";
import { useAppDispatch, useAppSelector } from "../../../app/hooks";
import { login, clearError } from "../slice/authSlice";
import { useNavigate } from "react-router-dom";

const LoginForm = () => {
  const dispatch = useAppDispatch();
  const navigate = useNavigate();
  const { error, loginLoading } = useAppSelector((state) => state.auth);

  const [form, setForm] = useState({
    email: "",
    password: "",
  });

  const handleSubmit = async (
    e: React.SyntheticEvent<HTMLFormElement>): Promise<void> => {
    e.preventDefault();

    const result = await dispatch(login(form));

    if (login.fulfilled.match(result)) {
      navigate("/user");
    }
  };

  return (
    <form onSubmit={handleSubmit} className="space-y-4">
      <input
        type="email"
        placeholder="Email"
        required
        className="w-full border border-gray-300 rounded px-3 py-2 focus:outline-none focus:ring-2 focus:ring-blue-500"
        onChange={(e) => {
          dispatch(clearError());
          setForm({ ...form, email: e.target.value });
        }}
      />

      <input
        type="password"
        placeholder="Password"
        required
        className="w-full border border-gray-300 rounded px-3 py-2 focus:outline-none focus:ring-2 focus:ring-blue-500"
        onChange={(e) => {
          dispatch(clearError());
          setForm({ ...form, password: e.target.value });
        }}
      />

      <button
        type="submit"
        disabled={loginLoading}
        className="w-full bg-blue-600 text-white py-2 rounded hover:bg-blue-700 transition disabled:bg-gray-400"
      >
        {loginLoading ? "Logging in..." : "Login"}
      </button>

      {error && (
        <div className="bg-red-100 border border-red-400 text-red-700 px-4 py-2 rounded">
          {error.detail && <p>{error.detail}</p>}

          {error.errors &&
            Object.entries(error.errors).map(([field, messages]) =>
              messages.map((msg, index) => (
                <p key={field + index}>{msg}</p>
              ))
            )}

          {!error.detail && !error.errors && error.title && (
            <p>{error.title}</p>
          )}
        </div>
      )}
    </form>
  );
};

export default LoginForm;