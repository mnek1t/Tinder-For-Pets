import React, {useState} from "react";
import lock from "../assets/lock.svg"
import ConfirmedWindow from "./ConfirmedWindow";
import "../styles/form.css"
import {forgotPassword} from "../api/authApi"

export default function ForgotPasswordForm() {
    const [email, setEmail] = useState<string>("");
    const [isEmailSent, setIsEmailsent] = useState<boolean>(false);
    function sendLoginLink(e: React.FormEvent<HTMLFormElement>) {
        e.preventDefault();
        forgotPassword(email)
        .then(() => {
            setIsEmailsent(true);
        })
    }

    function handleInput(e: React.ChangeEvent<HTMLInputElement>) {
        setEmail(e.target.value);
    }

    return (
        <>
            {isEmailSent ? (
                <ConfirmedWindow title="Your reset link has been sent!" message="Go to your email and follow the instructions!"/> 
            ) : (
                <form className='reset-password' onSubmit={(e) => sendLoginLink(e)}>
                    <div className='reset-password-container'>
                        <img className="form-image" src={lock} alt="lock logo" />
                        <h1 className="reset-password__header">Trouble logging in?</h1>
                        <p className="forgot-password-hint">
                            Enter your email and we'll send you a link to get back into your account.
                        </p>
                        <input
                            className="reset-password__input"
                            placeholder="Email"
                            value={email}
                            name="email"
                            onChange={handleInput}
                            required
                        />
                        <button className="reset-password__button" type='submit'>Send login link</button>
                        <br />
                        <div className="or-alt">
                            <div className="line"></div>
                            <div className="text">OR</div>
                            <div className="line"></div>
                        </div>
                        <br />
                        <div>
                            <a className="create-account-link" href="/register">Create New Account</a>
                        </div>
                    </div>
                    <div className="back-2-login-container">
                        <a className="back-2-login-link" href="/login">Back to login</a>
                    </div>
                </form>
            )}
        </>
    );    
}