import CreateProfileForm from "../components/CreateProfileForm"
import EmptyHeader from "../components/EmptyHeader";
import {createPetProfile, ProfileData} from "../api/profileApi";
import { useState } from "react";
export default function CreateProfilePage() {
    function createProfile(event : React.FormEvent<HTMLFormElement>, data: ProfileData) {
        event.preventDefault();
        console.log(event)
        createPetProfile(data);
    }

    return(
        <>
        <EmptyHeader/>
        <CreateProfileForm createProfile={createProfile}/>
        </>
    );
}