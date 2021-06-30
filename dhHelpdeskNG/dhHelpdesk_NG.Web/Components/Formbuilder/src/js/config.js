import mi18n from 'mi18n'

const noop = () => null

// eslint-disable-next-line
mi18n.addLanguage('en-US', FB_EN_US)

export const defaultOptions = {
  actionButtons: [], // additional form action buttons- save, data, clear
  allowStageSort: true,
  append: false, // append non-editable content to the form.
  controlOrder: [
    'text',
    'textarea',
    'select',
    'autocomplete',
    'button',
    'checkbox-group',
    'checkbox',
    'date',
    'file',
    'header',
    'hidden',
    'number',
    'paragraph',
    'radio-group',
  ],
  controlPosition: 'right',
  dataType: 'json',
  defaultFields: [],
  disabledActionButtons: ['save', 'data'], // save, data, clear
  disabledAttrs: [
    'access',
    'className',
    'description',
    'inline',
    'max',
    'maxlength',
    'min',
    'multiple',
    'name',
    'other',
    'rows',
    'step',
    'style',
    'subtype',
    'toggle',
  ],
  disabledFieldButtons: {},
  disabledSubtypes: {}, // subtypes you want disabled
  disableFields: [
    'autocomplete',
    'button',
    'checkbox-group',
    'file',
    'select',
    'header',
    'hidden',
    'number',
    'paragraph',
    'radio-group',
  ], // Array of fields to disable
  disableHTMLLabels: false, // disables html in field labels
  disableInjectedStyle: false, // removes the injected style
  editOnAdd: false, // opens the edit panel on added field
  fields: [], // add custom control configs
  fieldRemoveWarn: false,
  fieldEditContainer: null, // DOM node or selector
  inputSets: [], // add groups of fields at a time
  notify: {
    error: error => {
      console.log(error)
    },
    success: success => {
      console.log(success)
    },
    warning: warning => {
      console.warn(warning)
    },
  },
  onAddField: (fieldId, fieldData) => fieldData,
  onAddFieldAfter: (fieldId, fieldData) => fieldData,
  onAddOption: obj => obj,
  onClearAll: noop,
  onCloseFieldEdit: noop,
  onOpenFieldEdit: noop,
  /**
   * @param {Object} evt
   * @param {Object} formData
   */
  onSave: noop,
  persistDefaultFields: false,
  prepend: false,
  replaceFields: [],
  roles: {
    1: 'Administrator',
  },
  scrollToFieldOnAdd: true,
  showActionButtons: true,
  sortableControls: false,
  stickyControls: {
    enable: true,
    offset: {
      top: 5,
      bottom: 'auto',
      right: 'auto',
    },
  },
  subtypes: {},
  templates: {},
  typeUserAttrs: {},
  typeUserDisabledAttrs: {},
  typeUserEvents: {},
}

export const styles = {
  btn: ['default', 'danger', 'info', 'primary', 'success', 'warning'],
}

export const defaultI18n = {
  location: 'assets/lang/',
}

export const config = {}
