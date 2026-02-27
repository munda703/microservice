import api from "@/lib/axios";
import type { RegisterDTO, LoginDTO } from "../types";

export const registerUser = async (data: RegisterDTO) => {
  debugger
  console.log("intial")
  const response = await api.post("/gateWay/register", data);
console.log("after")
  return response.data;
};

export const loginUser = async (data: LoginDTO) => {
  const response = await api.post("/gateWay/login", data);
  return response.data;
};

export const getMyDetails = async () => {
  const response = await api.get("/gateWay/mydetails");
  return response.data;
};
