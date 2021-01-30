import { createStyles, makeStyles, Theme } from '@material-ui/core/styles';

export const useCalculationsFormStyles = makeStyles((theme: Theme) =>
  createStyles({
    root: {
      "& .MuiTextField-root": {
        margin: theme.spacing(1),
        width: "15%",
      },
      "& > *": {
        margin: theme.spacing(1),
      },
    },
    input: {
      display: "none",
    },
  })
);
