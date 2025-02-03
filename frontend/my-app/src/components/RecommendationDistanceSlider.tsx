import {useState, useEffect} from 'react';
import { FormControlLabel, Typography } from "@mui/material";
import CustomSlider from "./CustomSlider";
import CustomSwitch from "./CustomSwitch";

export default function RecommendationDistanceSlider() {
    const [showPetsInRange, setShowPetsInRange] = useState(true);
    const [distance, setDistance] = useState(50);

    // Load saved parameters from localStorage
    useEffect(() => {
        const savedDistanceSearch = localStorage.getItem('tinder-for-pets-distance-recs') || "";
        const parsedDistance = parseInt(savedDistanceSearch, 10);
        if (!isNaN(parsedDistance)) {
            setDistance(parsedDistance);
        }

        const savedDistanceFlag = localStorage.getItem('tinder-for-pets-distance-flag');
        if (savedDistanceFlag === "true") {
            setShowPetsInRange(true);
        } else if (savedDistanceFlag === "false") {
            setShowPetsInRange(false);
        }
    }, []);

    const handleDistanceChange = (newDistance : number) => {
        setDistance(newDistance);
        localStorage.setItem('tinder-for-pets-distance-recs', newDistance.toString());
    };

    const handleStrictRange = (isStrict : boolean) => {
        setShowPetsInRange(isStrict);
        localStorage.setItem('tinder-for-pets-distance-flag', isStrict.toString());
    };

    return(
        <>
            <div>
                <Typography gutterBottom 
                    sx={{
                        fontFamily: "'Karla', sans-serif", 
                        fontSize: "14px",                  
                        ".MuiFormControlLabel-label": {
                        fontFamily: "'Karla', sans-serif", 
                        },
                    }}>
                        Distance Preference {distance}km
                </Typography>
                <CustomSlider aria-label="ios slider" value={distance}  valueLabelDisplay="on" onChange={(e, value) => handleDistanceChange(value as number)}/>

                <div className="toggler-preference">
                        <FormControlLabel
                            control={
                                <CustomSwitch name="DistanceToggler" checked={showPetsInRange} onChange={(e, value) => handleStrictRange(value as boolean)}/>
                            }
                            label="Only show pets in this range"
                            labelPlacement="start"
                            sx={{
                                fontFamily: "'Karla', sans-serif", 
                                fontSize: "14px",                  
                                ".MuiFormControlLabel-label": {
                                    fontFamily: "'Karla', sans-serif", 
                                },
                            }}
                        />
                </div>    
            </div>
        </>
    )
}