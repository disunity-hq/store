import { ChangeEventArgs } from '@syncfusion/ej2/dropdowns';

export function NavigateToVersion(args: ChangeEventArgs) {
  if (args.value) {
    window.location.href = args.value + '';
  }
}
