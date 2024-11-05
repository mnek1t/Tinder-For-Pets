import About from "./pages/About";
import Home from "./pages/Home";
import NoPage from "./pages/NoPage";
import ForgotPasswordPage from "./pages/ForgotPasswordPage";
import LoginPage from "./pages/LoginPage";
import RegisterPage from "./pages/RegisterPage";
import ResetPasswordPage from "./pages/ResetPasswordPage";
import CreateProfilePage from "./pages/CreateProfilePage";
import ProfilePage from "./pages/ProfilePage";
import { BrowserRouter,Routes, Route } from "react-router-dom";
function App() {
  return (
    <div className="App">
      <BrowserRouter>
      <Routes>
        <Route path="/" element={<About/>}/>
        <Route path="/about" element={<About/>}/>
        <Route path="/login" element={<LoginPage />} />
        <Route path="/register" element={<RegisterPage />} />
        <Route path="/home" element={<Home/>}/>
        <Route path="*" element={<NoPage/>}/>
        <Route path="/accounts/password/forgot" element={<ForgotPasswordPage/>}/>
        <Route path="/accounts/password/reset" element={<ResetPasswordPage/>}/>
        <Route path="/profile/create" element={<CreateProfilePage/>}/>
        <Route path="/app/profile" element={<ProfilePage/>}/>
      </Routes>
      </BrowserRouter>
    </div>
  );
}

export default App;
