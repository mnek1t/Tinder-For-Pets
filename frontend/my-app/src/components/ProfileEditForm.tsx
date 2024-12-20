import { useState } from "react";
import { ProfileDetailsData } from "../api/profileApi";
import { CircularProgress, FormControlLabel } from '@mui/material';
import FileUploader from "./FileUploader";
import CustomSwitch from "../CustomSwitch";
interface ProfileEditFormProps {
    profileDetails : ProfileDetailsData | null,
    animalTypes: Array<{ id: number; typeName: string }>;
    breeds: Array<{ id: number; breedName: string }>;
    handleClickEditProfileCard : (event: React.MouseEvent<HTMLButtonElement>) => void;
}

const ProfileEditForm = (props: ProfileEditFormProps) => {
  const [formData, setFormData] = useState({
    name: props.profileDetails?.profile.name || "",
    dateOfBirth: props.profileDetails?.profile.dateOfBirth || "",
    description: props.profileDetails?.profile.description || "",
    typeId: props.animalTypes.find(type => type.typeName === props.profileDetails?.animal.type)?.id || "",
    breedId: props.breeds.find(breed => breed.breedName === props.profileDetails?.animal.breed)?.id || "",
    isVaccinated: props.profileDetails?.profile.isVaccinated || false,
    isSterilized: props.profileDetails?.profile.isSterilized || false,
  });

    function handleImageUpload(event : React.ChangeEvent<HTMLInputElement>) {
      const { files } = event.target;
      if (files && files[0]) {
          console.log("Image uploaded:", files[0]);
          // Logic to upload/set the image file
      }
    }

    const handleInputChange = (e: React.ChangeEvent<HTMLInputElement | HTMLSelectElement>) => {
      const { name, value } = e.target;
      setFormData((prev) => ({
          ...prev,
          [name]: value,
      }));
  };

  const handleSwitchChange = (e: React.ChangeEvent<HTMLInputElement>) => {
      const { name, checked } = e.target;
      setFormData((prev) => ({
          ...prev,
          [name]: checked,
      }));
  };

  const handleContentEditableChange = (e: React.FocusEvent<HTMLHeadingElement | HTMLParagraphElement>, key: string) => {
      const value = e.currentTarget.textContent || "";
      setFormData((prev) => ({
          ...prev,
          [key]: value,
      }));
  };

    function createImageFileFromBase64() {
        const base64 = props.profileDetails?.images?.[0]?.imageData || '';
        const mimeType = props.profileDetails?.images?.[0]?.imageFormat || '';
        const binaryData = atob(base64);
        const byteArray = new Uint8Array(binaryData.length);

        for (let i = 0; i < binaryData.length; i++) {
            byteArray[i] = binaryData.charCodeAt(i);
        }
        return new File([byteArray], 'profileImage', { type: mimeType });
    }

    const selectedTypeId = props.animalTypes.find(
      (type) => type.typeName === props.profileDetails?.animal.type
    )?.id;
    
    const selectedBreedId = props.breeds.find(
      (breed) => breed.breedName=== props.profileDetails?.animal.breed
    )?.id;

    const handleSubmit = (e: React.FormEvent) => {
      e.preventDefault();
      console.log("Form submitted:", formData);
      // Submit updated formData to the backend or state manager
    };

    return(
        <>
            <form onSubmit={handleSubmit}>
              <div className="profile-card">
                <FileUploader handleFileUpload={handleImageUpload} maxSizeMB="10" accept="image/*" initialImage={createImageFileFromBase64()}/>
                  <div style={{display:'flex',  justifyContent:'space-around', alignItems:'center'}}>
                    <h1 className="edit-profile-card__name" contentEditable suppressContentEditableWarning onBlur={(e) => handleContentEditableChange(e, "name")}>
                      {props.profileDetails?.profile.name}
                    </h1>

                    <input type="date" name="dateOfBirth" value={formData.dateOfBirth} className="profile-card__dob" onChange={handleInputChange}/>
                  </div>

                  <div style={{display:'flex',  justifyContent:'space-around', alignItems:'center'}}>
                    <div>
                      <label htmlFor="typeId">Animal Type</label>
                      <select name="typeId" value={selectedTypeId} className="create-profile-form__animal-type" onChange={handleInputChange} required>
                      {props.animalTypes.map((type) => (
                          <option key={type.id} value={type.id}>
                              {String(type.typeName).charAt(0).toUpperCase() + String(type.typeName).slice(1)}
                          </option>
                      ))}
                      </select>
                    </div>

                    <span>,</span>

                    <div>
                      <label htmlFor="breedId">Breed</label>
                      <select name="breedId" value={selectedBreedId} className="create-profile-form__breed" onChange={handleInputChange} required>
                          {props.breeds.map((breed) => (
                              <option key={breed.id} value={breed.id}>
                                  {breed.breedName}
                              </option>
                          ))}
                      </select>
                    </div>
                  </div>

                  <p contentEditable suppressContentEditableWarning onBlur={(e) => handleContentEditableChange(e, "description")}>
                    {props.profileDetails?.profile.description}
                  </p>

                  <FormControlLabel
                    control={
                      <CustomSwitch name="isVaccinated" checked={props.profileDetails?.profile.isVaccinated} onChange={handleSwitchChange}/>
                    }
                    label="Vacinated:"
                    labelPlacement="start"
                    sx={{
                      fontFamily: "'Karla', sans-serif", 
                      fontSize: "14px",                 
                      ".MuiFormControlLabel-label": {
                        fontFamily: "'Karla', sans-serif", 
                      },
                    }}
                  />
                  <FormControlLabel
                    control={
                      <CustomSwitch name="isSterilized" checked={props.profileDetails?.profile.isSterilized} onChange={handleSwitchChange}/>
                    }
                    label="Sterialized:"
                    labelPlacement="start"
                    sx={{
                      fontFamily: "'Karla', sans-serif", 
                      fontSize: "14px",                  
                      ".MuiFormControlLabel-label": {
                        fontFamily: "'Karla', sans-serif", 
                      },
                    }}
                  />
                  <button type="submit" name="update-profile-btn" className="edit-profile-btn">Save</button>
                  <button type="button" name="cancel-update-profile-btn" className="edit-profile-btn" onClick={props.handleClickEditProfileCard}>Cancel</button>
              </div>
            </form>
        </>
    );
}

export default ProfileEditForm;