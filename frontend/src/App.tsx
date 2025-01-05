import { Outlet } from "react-router";
import "./App.css";
import Navbar from "./Components/Navbar/Navbar";
import "react-toastify/dist/ReactToastify.css";
import { ToastContainer } from "react-toastify";
import { UseProvider } from "./Context/userAuth";

function App() {
  return (
    <>
    <UseProvider>
      <Navbar />
      <Outlet />
      <ToastContainer />
    </UseProvider>
    </>
  );
}

export default App;
