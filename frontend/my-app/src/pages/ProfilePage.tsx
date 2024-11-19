
// TODO: IMPLEMENT GET DATA IN PAGE AND IF ERROR DO NOT RENDER ANYTHING
import Sidebar from "../components/Sidebar";
import ProfileCard from "../components/profile/ProfileCard";
import FormPageWrapper from '../components/FormPageWrapper';
import { deleteAccount } from "../api/authApi";
import { logout } from "../api/authApi"
import { useNavigate } from "react-router-dom";
import React, { useState, useEffect } from "react";
import ConfirmationPopup from "../components/ConfirmationPopup";

const deleteAccountDescription = "If you delete your account, you will permanently lose your profile, messages, photos and matches. If you delete your account, this action cannot be undone."

export default function ProfilePage() {
    const [distance, setDistance] = useState(50);
    const [showPetsInRange, setShowPetsInRange] = useState(true);
    const [showConfirmationWindow, setShowConfirmationWindow ] = useState<boolean>(false);
    const navigate = useNavigate();
    
    useEffect(() => {
        document.body.style.background = '#FEFAF6';

        return () => {
            document.body.style.backgroundImage = "";
            document.body.style.backgroundSize = '';
        };
    }, []);
    
    function handleLogout(event : React.MouseEvent<HTMLButtonElement>) {
        logout();
        navigate("/about");
    }

    function handleDeleteAccountButtonClick(event : React.MouseEvent<HTMLButtonElement>) {
        setShowConfirmationWindow(true);
    }

    function toggleShowPets() {
        setShowPetsInRange((prev) => !prev);
    }

    function enableLightMode() {
        console.log("Light mode activated");
    }

    function enableDarkMode() {
        console.log("Dark mode activated");
      }

    function handleDeleteAccount(event : React.MouseEvent<HTMLButtonElement>) {
        const {name} = event.target as HTMLButtonElement;
        if(name === 'cancel') 
        {
            setShowConfirmationWindow(false);
        } else if(name === 'confirm'){
            deleteAccount();
            navigate("/about");
            setShowConfirmationWindow(false);
        }
    }

    return(
        <>
            <div className="container">
                <div className="sidebar-container">
                    <Sidebar 
                        isUserPreferences = {true}
                        handleLogout={handleLogout} 
                        handleDeleteAccount={handleDeleteAccountButtonClick} 
                        distance={distance}
                        onDistanceChange={setDistance}
                        showPetsInRange={showPetsInRange}
                        onToggleShowPets={toggleShowPets}
                        onLightMode={enableLightMode}
                        onDarkMode={enableDarkMode}/>
                </div>
                
                <div className="profile">
                    <ProfileCard />
                </div>
                {
                    showConfirmationWindow && 
                    <FormPageWrapper title="delete-account" showHeader={false}>
                        <ConfirmationPopup question="You sure you want to delete an account?" description={deleteAccountDescription} handleAction={handleDeleteAccount}/> 
                    </FormPageWrapper>
                }
            </div>       
            {/* {isDataReceived && <div className="container">
                <div className="sidebar-container">
                    <Sidebar handleLogout={handleLogout} handleDeleteAccount={handleDeleteAccount}/>
                </div>
                
                <div className="profile">
                    <ProfileCard />
                </div>
            </div>} */}
        </>
        );
}