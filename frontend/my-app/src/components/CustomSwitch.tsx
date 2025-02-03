import { Switch, SwitchProps, styled } from "@mui/material";
interface CustomSwitchProps extends SwitchProps {
  isDisabled?: boolean;
}
const CustomSwitch = styled((props: CustomSwitchProps) => {
  const { isDisabled, ...restProps } = props;
  return <Switch {...restProps} />;
})(({ theme }) => ({
  fontFamily: 'Karla',
  width: 42,
  height: 26,
  padding: 0,
  margin: "10px",
  "& .MuiSwitch-switchBase": {
    padding: 0,
    margin: 2,
    transitionDuration: "300ms",
    "&.Mui-checked": {
      transform: "translateX(16px)",
      color: "#fff",
      "& + .MuiSwitch-track": {
        backgroundColor: "#A75D5D",
        opacity: 1,
        border: 0,
      },
      "&.Mui-disabled + .MuiSwitch-track": {
        opacity: 0.5,
      },
    },
    "&.Mui-focusVisible .MuiSwitch-thumb": {
      color: "#33cf4d",
      border: "6px solid #fff",
    },
    "&.Mui-disabled .MuiSwitch-thumb": {
      color: theme.palette.grey[100],
      ...(theme.palette.mode === "dark" && {
        color: theme.palette.grey[600],
      }),
    },
    "&.Mui-disabled + .MuiSwitch-track": {
      opacity: 0.7,
      ...(theme.palette.mode === "dark" && {
        opacity: 0.3,
      }),
    },
  },
  "& .MuiSwitch-thumb": {
    boxSizing: "border-box",
    width: 22,
    height: 22,
  },
  "& .MuiSwitch-track": {
    borderRadius: 26 / 2,
    backgroundColor: "#E9E9EA",
    opacity: 1,
    transition: theme.transitions.create(["background-color"], {
      duration: 500,
    }),
    ...(theme.palette.mode === "dark" && {
      backgroundColor: "#39393D",
    }),
  },
}));

export default CustomSwitch;