import RegisterForm from "../features/usersAuth/components/RegisterForm";
import { Link } from "react-router-dom";

const RegisterPage = () => {
  return (
    <div className="flex justify-center items-center min-h-[70vh]">
      <div className="bg-white shadow-xl rounded-2xl p-8 w-full max-w-md">
        <h1 className="text-2xl font-bold text-center mb-2">
          Create Your Account
        </h1>

        <p className="text-gray-500 text-sm text-center mb-6">
          Join the Wonga Assessment platform and demonstrate your authentication implementation.
        </p>

        <RegisterForm />

        <p className="text-sm text-center mt-6">
          Already registered?{" "}
          <Link
            to="/login"
            className="text-blue-600 font-medium hover:underline"
          >
            Login here
          </Link>
        </p>
      </div>
    </div>
  );
};

export default RegisterPage;