import React from 'react'
import * as Yup from "yup";
import { yupResolver } from "@hookform/resolvers/yup";
import { useAuth } from "../../Context/userAuth";
import { useForm } from "react-hook-form";

type Props = {};

type RegisterFormInputs = {
    email: string;
    userName: string;
    password: string;

};

const validation = Yup.object().shape({
    email: Yup.string().email("Invalid Email Format").required("Email is required"),
    userName: Yup.string().required("Username is required"),
    password: Yup.string().required("Password is required")
});

const RegisterPage = (props : Props) => {
    const { registerUser } = useAuth();
    const { register, handleSubmit, formState: { errors }} = useForm<RegisterFormInputs>();
    //{ resolver: yupResolver(validation)}
        
    const handleRegister = (form: RegisterFormInputs) => {
        registerUser(form.userName, form.email, form.password);
    };
    return (
        <section className="bg-slate-900">
          <div className="flex flex-col items-center justify-center px-6 py-8 mx-auto md:h-screen lg:py-0">
            <div className="w-full rounded-lg shadow dark:border md:mb-20 sm:max-w-md xl:p-0 bg-slate-800 border-slate-700">
              <div className="p-6 space-y-4 md:space-y-6 sm:p-8">
                <h1 className="text-xl font-bold leading-tight tracking-tight md:text-2xl text-white">
                  Sign in to your account
                </h1>
                <form className="space-y-4 md:space-y-6" onSubmit={handleSubmit(handleRegister)}>
                <div>
                    <label
                      
                      className="block mb-2 text-sm font-medium text-white"
                    >
                      Email
                    </label>
                    <input
                      type="text"
                      id="email"
                      className="bg-gray-50 border border-gray-300 text-gray-900 sm:text-sm rounded-lg focus:ring-primary-600 focus:border-primary-600 block w-full p-2.5 dark:bg-gray-700 dark:border-gray-600 dark:placeholder-gray-400 dark:text-white dark:focus:ring-blue-500 dark:focus:border-blue-500"
                      placeholder="Email"
                      {...register("email")}
                    />
                    {errors.email ? <p className="text-white">{errors.email.message}</p> : ""}
                  </div>
                  <div>
                    <label
                      htmlFor="username"
                      className="block mb-2 text-sm font-medium text-white"
                    >
                      Username
                    </label>
                    <input
                      type="text"
                      id="username"
                      className="bg-gray-50 border border-gray-300 text-gray-900 sm:text-sm rounded-lg focus:ring-primary-600 focus:border-primary-600 block w-full p-2.5 dark:bg-gray-700 dark:border-gray-600 dark:placeholder-gray-400 dark:text-white dark:focus:ring-blue-500 dark:focus:border-blue-500"
                      placeholder="Username"
                      {...register("userName")}
                    />
                    {errors.userName ? <p className="text-white">{errors.userName.message}</p> : ""}
                  </div>
                  <div>
                    <label
                      htmlFor="password"
                      className="block mb-2 text-sm font-medium text-white"
                    >
                      Password
                    </label>
                    <input
                      type="password"
                      id="password"
                      placeholder="••••••••"
                      className="bg-gray-50 border border-gray-300 text-gray-900 sm:text-sm rounded-lg focus:ring-primary-600 focus:border-primary-600 block w-full p-2.5 dark:bg-gray-700 dark:border-gray-600 dark:placeholder-gray-400 dark:text-white dark:focus:ring-blue-500 dark:focus:border-blue-500"
                      {...register("password")}
                    />
                    {errors.password ? <p className="text-white">{errors.password.message}</p> : ""}
                  </div>
                  
                  <button
                    type="submit"
                    className="w-full text-white bg-lightGreen hover:bg-primary-700 focus:ring-4 focus:outline-none focus:ring-primary-300 font-medium rounded-lg text-sm px-5 py-2.5 text-center dark:bg-primary-600 dark:hover:bg-primary-700 dark:focus:ring-primary-800"
                  >
                    Sign Up
                  </button>
                </form>
              </div>
            </div>
          </div>
        </section>
    );
}

export default RegisterPage
