
// TODO: IMPLEMENT GET DATA IN PAGE AND IF ERROR DO NOT RENDER ANYTHING
import React, { useState, useEffect } from "react";
import Sidebar from "../components/Sidebar";
import ProfileCard from "../components/profile/ProfileCard";
import FormPageWrapper from '../components/FormPageWrapper';
import ConfirmationPopup from "../components/ConfirmationPopup";
import ProfileEditForm from "../components/ProfileEditForm";
import { logout, deleteAccount } from "../api/authApi";
import { getProfileDetails, ProfileDetailsData, getAnimalTypes, getBreeds } from "../api/profileApi";
import { useNavigate } from "react-router-dom";
import { CircularProgress } from "@mui/material";
import { DEFAULT_ANIMAL_TYPE_ID } from "../utils/TinderConstants";

const deleteAccountDescription = "If you delete your account, you will permanently lose your profile, messages, photos and matches. If you delete your account, this action cannot be undone."

export default function ProfilePage() {
    const [animalTypes, setAnimalTypes] = useState([]);
    const [breeds, setBreeds] = useState([]);
    const [distance, setDistance] = useState(50);
    const [showPetsInRange, setShowPetsInRange] = useState(true);
    const [showConfirmationWindow, setShowConfirmationWindow ] = useState<boolean>(false);
    const [profileDetails, setProfileDetails] = useState<ProfileDetailsData | null>(null);
    const [loading, setLoading] = useState(false);
    const [isEditing, setIsEditing] = useState(false);
    const navigate = useNavigate();

    useEffect(() => {
            getAnimalTypes()
              .then((animalTypes) => {
                setAnimalTypes(animalTypes);
                handleFetchBreeds(animalTypes[0]?.id || profileDetails?.animal.type);
            })
            .catch((err) => console.error(err));
        }, []);
    const handleFetchBreeds = (typeId: number) => {
        getBreeds(typeId)
            .then(setBreeds)
            .catch((err) => console.error(err));
    };
    useEffect(() => {
        document.body.style.background = '#FEFAF6';
        setLoading(true);
            getProfileDetails()
            .then(({ profile, animal, images }) => {
            setProfileDetails({
                    animal: {
                    type: animal?.animalType || "", 
                    breed: animal?.breed || "", 
                    },
                    profile: {
                    id: profile?.id || "", 
                    name: profile?.name || "",
                    description: profile?.description || "",
                    age: profile?.age || 0,
                    sex: profile?.sex || "",
                    isVaccinated: profile?.isVaccinated || false,
                    isSterilized: profile?.isSterilized || false,
                    },
                    images: [
                    {
                        imageData: images[0]?.imageData || "", 
                        imageFormat: images[0]?.imageFormat || "", 
                    },
                    ],
                });
            })
            .catch(error => {
                console.error('Error fetching profile details:', error);
            })
            .finally(() => {
                setLoading(false);
            })
        return () => {
            document.body.style.backgroundImage = "";
            document.body.style.backgroundSize = '';
        };
    }, []);
    
    function handleLogout(event : React.MouseEvent<HTMLButtonElement>) {
        setLoading(true);
        logout()
        .then(() => {
            navigate("/about");
        })
        .catch((error) => {
            console.error(error);
        })
        .finally(() => setLoading(false));
        
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

    function handleClickEditProfileCard(event : React.MouseEvent<HTMLButtonElement> ) {
        setIsEditing((prev) => !prev)
    }

    return(
        <>
            <div className="container">
                {loading ? (
                    <FormPageWrapper title="delete-account" showHeader={false}>
                        <CircularProgress/> 
                    </FormPageWrapper>) :
                    (
                        <>
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
                                {isEditing ? <ProfileEditForm animalTypes={animalTypes} breeds={breeds} profileDetails={profileDetails} handleClickEditProfileCard={handleClickEditProfileCard}/> : <ProfileCard profileDetails={profileDetails} loading={loading} handleClickEditProfileCard={handleClickEditProfileCard}/>}
                            </div>
                            {
                                showConfirmationWindow && 
                                <FormPageWrapper title="delete-account" showHeader={false}>
                                    <ConfirmationPopup question="You sure you want to delete an account?" description={deleteAccountDescription} handleAction={handleDeleteAccount}/> 
                                </FormPageWrapper>
                            }
                        </>
                    )
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