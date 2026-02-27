import { useEffect } from "react";
import { useAppDispatch, useAppSelector } from "../app/hooks";
import { fetchUser, logout } from "../features/usersAuth/slice/authSlice";
import { useNavigate } from "react-router-dom";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faUserCircle } from "@fortawesome/free-solid-svg-icons";

const UserDetailsPage = () => {
  const dispatch = useAppDispatch();
  const navigate = useNavigate();
  const { user } = useAppSelector((state) => state.auth);

  useEffect(() => {
    dispatch(fetchUser());
  }, [dispatch]);

  const handleLogout = () => {
    dispatch(logout());
    navigate("/login");
  };

  if (!user) {
    return (
      <div className="flex justify-center py-24">
        <p className="text-gray-500 text-lg">Loading user details...</p>
      </div>
    );
  }

  return (
    <div className="flex justify-center py-24 px-6">
      <div className="bg-white shadow-2xl rounded-2xl p-14 w-full max-w-3xl border border-gray-100">

        {/* Header Section */}
        <div className="flex items-center gap-5 mb-10">
          <FontAwesomeIcon
            icon={faUserCircle}
            className="text-blue-500 text-5xl"
          />

          <div>
            <p className="text-sm text-gray-500">
              Welcome back
            </p>

            <h2 className="text-3xl font-bold text-gray-800 break-words">
              {user.firstName}
            </h2>
          </div>
        </div>

        <div className="border-t border-gray-200 mb-8"></div>

        <div className="space-y-6 text-gray-700 text-lg">
          <div className="flex justify-between border-b pb-3">
            <span className="font-semibold">First Name</span>
            <span className="break-words">{user.firstName}</span>
          </div>

          <div className="flex justify-between border-b pb-3">
            <span className="font-semibold">Last Name</span>
            <span className="break-words">{user.lastName}</span>
          </div>

          <div className="flex justify-between pb-3">
            <span className="font-semibold">Email</span>
            <span className="break-all text-right">
              {user.email}
            </span>
          </div>
        </div>

        <button
          onClick={handleLogout}
          className="mt-12 w-full bg-red-500 text-white py-3 rounded-lg hover:bg-red-600 transition font-medium"
        >
          Logout
        </button>

      </div>
    </div>
  );
};

export default UserDetailsPage;