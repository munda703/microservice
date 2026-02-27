import LoginForm from "../features/usersAuth/components/LoginForm";
import { Link } from "react-router-dom";

const LoginPage = () => {
  return (
    <div className="flex justify-center items-center min-h-[70vh]">
      <div className="bg-white shadow-lg rounded-lg p-8 w-full max-w-md">
        <h2 className="text-2xl font-semibold text-gray-800 text-center mb-2">
          Welcome Back
        </h2>

        <p className="text-sm text-gray-500 text-center mb-6">
          Please sign in to continue.
        </p>

        <p className="text-gray-600 text-sm mb-6 text-center">
          The primary objective of this assessment is to evaluate your
          understanding of the login process in a web application.
        </p>

        <LoginForm />

        <p className="text-sm text-center mt-4">
          Don’t have an account?{" "}
          <Link
            to="/register"
            className="text-blue-600 hover:underline font-medium"
          >
            Register here
          </Link>
        </p>
      </div>
    </div>
  );
};

export default LoginPage;