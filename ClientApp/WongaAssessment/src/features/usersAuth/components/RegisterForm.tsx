import { useState } from "react";
import { useAppDispatch, useAppSelector } from "../../../app/hooks";
import { register, clearError } from "../slice/authSlice";
import { useNavigate } from "react-router-dom";

const RegisterForm = () => {
  const dispatch = useAppDispatch();
  const navigate = useNavigate();

  const { error, registerLoading } = useAppSelector(
    (state) => state.auth
  );

  const [form, setForm] = useState({
    firstName: "",
    lastName: "",
    email: "",
    password: "",
  });

  const handleSubmit = async (e: React.SyntheticEvent<HTMLFormElement>): Promise<void> => {
    e.preventDefault();

    const result = await dispatch(register(form));

    if (register.fulfilled.match(result)) {
      navigate("/login");
    }
  };

  return (
    <form onSubmit={handleSubmit} className="space-y-4">

      <div>
        <label className="block text-sm font-medium mb-1">
          First Name
        </label>
        <input
          type="text"
          required
          className="w-full border border-gray-300 rounded-lg px-3 py-2 focus:ring-2 focus:ring-blue-500 focus:outline-none"
          onChange={(e) => {
            dispatch(clearError());
            setForm({ ...form, firstName: e.target.value });
          }}
        />
      </div>

      <div>
        <label className="block text-sm font-medium mb-1">
          Last Name
        </label>
        <input
          type="text"
          required
          className="w-full border border-gray-300 rounded-lg px-3 py-2 focus:ring-2 focus:ring-blue-500 focus:outline-none"
          onChange={(e) => {
            dispatch(clearError());
            setForm({ ...form, lastName: e.target.value });
          }}
        />
      </div>

      <div>
        <label className="block text-sm font-medium mb-1">
          Email Address
        </label>
        <input
          type="email"
          required
          className="w-full border border-gray-300 rounded-lg px-3 py-2 focus:ring-2 focus:ring-blue-500 focus:outline-none"
          onChange={(e) => {
            dispatch(clearError());
            setForm({ ...form, email: e.target.value });
          }}
        />
      </div>

      <div>
        <label className="block text-sm font-medium mb-1">
          Password
        </label>
        <input
          type="password"
          required
          className="w-full border border-gray-300 rounded-lg px-3 py-2 focus:ring-2 focus:ring-blue-500 focus:outline-none"
          onChange={(e) => {
            dispatch(clearError());
            setForm({ ...form, password: e.target.value });
          }}
        />
      </div>

      <button
        type="submit"
        disabled={registerLoading}
        className="w-full bg-blue-600 text-white py-2 rounded-lg hover:bg-blue-700 transition disabled:bg-gray-400"
      >
        {registerLoading ? "Creating account..." : "Create Account"}
      </button>

      {error && (
        <div className="bg-red-100 border border-red-400 text-red-700 px-4 py-2 rounded-lg mt-2">
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

export default RegisterForm;